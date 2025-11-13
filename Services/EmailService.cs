using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FlowTap.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(
                _configuration["Email:FromName"] ?? "FlowTap",
                _configuration["Email:FromEmail"] ?? "noreply@flowtap.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["Email:SmtpServer"] ?? "smtp.gmail.com",
                int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
                SecureSocketOptions.StartTls);
            
            // In production, use credentials from configuration
            // await smtp.AuthenticateAsync(username, password);
            
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}

