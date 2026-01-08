using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<ApiResponse> VerifyEmailAsync(VerifyEmailRequest request);
    Task<UserDto?> GetCurrentUserAsync(string userId);
}
