namespace Flowtap_Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendVerificationEmailAsync(string email, string verificationLink, string username = "");
    Task<bool> SendWelcomeEmailAsync(string email, string username = "");
}

