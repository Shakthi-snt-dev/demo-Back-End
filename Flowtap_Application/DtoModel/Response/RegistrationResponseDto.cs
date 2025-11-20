namespace Flowtap_Application.DtoModel.Response;

public class RegisterResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public bool EmailSent { get; set; }
    public string Message { get; set; } = string.Empty;
}

