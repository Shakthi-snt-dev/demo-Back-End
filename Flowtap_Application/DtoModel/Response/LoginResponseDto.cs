using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UserInfoDto? User { get; set; }
}

