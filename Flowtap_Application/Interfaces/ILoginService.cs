using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface ILoginService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> ValidatePasswordAsync(string password, string passwordHash, string? passwordSalt);
}

