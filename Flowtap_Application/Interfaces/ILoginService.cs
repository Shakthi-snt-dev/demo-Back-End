using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface ILoginService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> ValidatePasswordAsync(string password, string passwordHash, string? passwordSalt);
}

