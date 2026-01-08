using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IConfiguration configuration,
        IEmailService emailService)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new ApiResponse<LoginResponse>(false, "Invalid email or password", null);
        }

        if (!user.IsActive)
        {
            return new ApiResponse<LoginResponse>(false, "Account is disabled", null);
        }

        var token = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var userDto = await MapToUserDto(user);
        var expiration = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

        return new ApiResponse<LoginResponse>(true, null, new LoginResponse(token, refreshToken, expiration, userDto));
    }

    public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new ApiResponse<RegisterResponse>(false, "Email is already registered", null);
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            ClubId = request.ClubId,
            Role = request.ClubId.HasValue ? UserRole.Member : UserRole.SuperAdmin,
            EmailConfirmed = true // For demo purposes
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponse<RegisterResponse>(false, errors, null);
        }

        // If registering for a club, create member record
        if (request.ClubId.HasValue)
        {
            var member = new Member
            {
                Id = Guid.NewGuid(),
                ClubId = request.ClubId.Value,
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Status = MemberStatus.Pending,
                JoinedDate = DateTime.UtcNow,
                EmailVerified = true
            };

            _context.Members.Add(member);
            user.MemberId = member.Id;
            await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(user);
        }

        var userDto = await MapToUserDto(user);
        return new ApiResponse<RegisterResponse>(true, "Registration successful", new RegisterResponse(true, null, userDto));
    }

    public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
        {
            return new ApiResponse<LoginResponse>(false, "Invalid token", null);
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new ApiResponse<LoginResponse>(false, "Invalid or expired refresh token", null);
        }

        var newToken = await GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var userDto = await MapToUserDto(user);
        var expiration = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

        return new ApiResponse<LoginResponse>(true, null, new LoginResponse(newToken, newRefreshToken, expiration, userDto));
    }

    public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that user doesn't exist
            return new ApiResponse(true, "If the email exists, a reset link has been sent");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendPasswordResetEmailAsync(request.Email, token);

        return new ApiResponse(true, "If the email exists, a reset link has been sent");
    }

    public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new ApiResponse(false, "Invalid request");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponse(false, errors);
        }

        return new ApiResponse(true, "Password has been reset successfully");
    }

    public async Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse(false, "User not found");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponse(false, errors);
        }

        return new ApiResponse(true, "Password changed successfully");
    }

    public async Task<ApiResponse> VerifyEmailAsync(VerifyEmailRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return new ApiResponse(false, "Invalid request");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return new ApiResponse(false, "Invalid verification token");
        }

        return new ApiResponse(true, "Email verified successfully");
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return await MapToUserDto(user);
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new("role", user.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.ClubId.HasValue)
        {
            claims.Add(new Claim("clubId", user.ClubId.Value.ToString()));
        }

        if (user.MemberId.HasValue)
        {
            claims.Add(new Claim("memberId", user.MemberId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes());

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
            ValidateLifetime = false,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private int GetTokenExpirationMinutes()
    {
        return int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes) ? minutes : 15;
    }

    private async Task<UserDto> MapToUserDto(ApplicationUser user)
    {
        string? clubName = null;
        if (user.ClubId.HasValue)
        {
            var club = await _context.Clubs.FindAsync(user.ClubId.Value);
            clubName = club?.Name;
        }

        return new UserDto(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.FullName,
            user.Role,
            user.ClubId,
            user.MemberId,
            clubName
        );
    }
}
