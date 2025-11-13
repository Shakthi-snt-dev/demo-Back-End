using FlowTap.Api.DTOs;

namespace FlowTap.Api.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<UserDto> GetCurrentUserAsync(Guid userId);
    Task<UserDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
}

