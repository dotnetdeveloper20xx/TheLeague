using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

// Login
public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime Expiration,
    UserDto User
);

// Register
public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    [Required] string FirstName,
    [Required] string LastName,
    string? Phone,
    Guid? ClubId
);

public record RegisterResponse(
    bool Success,
    string? Message,
    UserDto? User
);

// Refresh Token
public record RefreshTokenRequest(
    [Required] string Token,
    [Required] string RefreshToken
);

// Password Reset
public record ForgotPasswordRequest(
    [Required, EmailAddress] string Email
);

public record ResetPasswordRequest(
    [Required, EmailAddress] string Email,
    [Required] string Token,
    [Required, MinLength(6)] string NewPassword
);

// Change Password
public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required, MinLength(6)] string NewPassword
);

// Verify Email
public record VerifyEmailRequest(
    [Required] string UserId,
    [Required] string Token
);

// User DTO
public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    UserRole Role,
    Guid? ClubId,
    Guid? MemberId,
    string? ClubName
);

// API Response wrapper
public record ApiResponse<T>(
    bool Success,
    string? Message,
    T? Data
);

public record ApiResponse(
    bool Success,
    string? Message
);
