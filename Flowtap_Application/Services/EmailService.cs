using Flowtap_Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Flowtap_Application.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string? _smtpHost;
    private readonly int _smtpPort;
    private readonly string? _smtpUsername;
    private readonly string? _smtpPassword;
    private readonly string? _smtpFromEmail;
    private readonly string? _smtpFromName;
    private readonly bool _smtpUseSsl;
    private readonly bool _smtpEnabled;
    private readonly string? _emailImageBaseUrl;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
        // Read SMTP configuration
        _smtpHost = _configuration["Smtp:Host"];
        _smtpPort = _configuration.GetValue<int>("Smtp:Port", 587);
        _smtpUsername = _configuration["Smtp:Username"];
        _smtpPassword = _configuration["Smtp:Password"];
        _smtpFromEmail = _configuration["Smtp:FromEmail"];
        _smtpFromName = _configuration["Smtp:FromName"] ?? "Flowtap";
        _smtpUseSsl = _configuration.GetValue<bool>("Smtp:UseSsl", true);
        _smtpEnabled = _configuration.GetValue<bool>("Smtp:Enabled", false);
        // Get base URL for email images - defaults to backend API URL
        var baseUrl = _configuration["AppSettings:BaseUrl"] ?? _configuration["AppSettings:EmailImageBaseUrl"] ?? "http://localhost:5113";
        _emailImageBaseUrl = baseUrl.TrimEnd('/');
        
        if (!_smtpEnabled)
        {
            _logger.LogWarning("SMTP is disabled in configuration. Emails will be logged only.");
        }
        else if (string.IsNullOrWhiteSpace(_smtpHost) || string.IsNullOrWhiteSpace(_smtpFromEmail))
        {
            _logger.LogWarning("SMTP configuration is incomplete. Emails will be logged only.");
        }
    }

    public async Task<bool> SendVerificationEmailAsync(string email, string verificationLink, string username = "")
    {
        try
        {
            // TODO: Implement actual email sending (SMTP, SendGrid, etc.)
            // For now, we'll just log it
            _logger.LogInformation("Sending verification email to {Email} with link: {Link}", email, verificationLink);

            var subject = "Verify Your Email - flow-tap";
            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <title>Verify Your Email - flow-tap</title>
    <!--[if mso]>
    <style type=""text/css"">
        body, table, td {{font-family: Arial, sans-serif !important;}}
    </style>
    <![endif]-->
</head>
<body style=""margin: 0; padding: 0; background-color: #f5f5f5; font-family: 'Nunito Sans', Arial, sans-serif;"">
    <!-- Wrapper Table -->
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""background-color: #f5f5f5;"">
        <tr>
            <td align=""center"" style=""padding: 40px 20px;"">
                <!-- Main Container -->
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""max-width: 600px; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                    
                    <!-- Header Section with Illustration -->
                    <tr>
                        <td class=""header-padding"" style=""background: linear-gradient(135deg, #4f66f1 0%, #6b82f5 100%); padding: 50px 30px; text-align: center;"">
                            <!-- Illustration Area -->
                            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"" style=""padding-bottom: 20px;"">
                                        <!-- Email Send Illustration -->
                                        <img src=""{_emailImageBaseUrl}/images/EmailService%20image.jpg"" alt=""Email Verification"" style=""display: block; width: 200px; height: 200px; border-radius: 50%; object-fit: cover; margin: 0 auto; margin-bottom: 20px;"" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align=""center"">
                                        <!-- Decorative Icons -->
                                        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""margin: 0 auto;"">
                                            <tr>
                                                <td style=""padding: 5px;"">
                                                    <div style=""width: 40px; height: 40px; background-color: rgba(255, 255, 255, 0.15); border-radius: 50%; display: inline-block; text-align: center; line-height: 40px; color: white; font-size: 18px;"">📊</div>
                                                </td>
                                                <td style=""padding: 5px;"">
                                                    <div style=""width: 40px; height: 40px; background-color: rgba(255, 255, 255, 0.15); border-radius: 50%; display: inline-block; text-align: center; line-height: 40px; color: white; font-size: 18px;"">📈</div>
                                                </td>
                                                <td style=""padding: 5px;"">
                                                    <div style=""width: 40px; height: 40px; background-color: rgba(255, 255, 255, 0.15); border-radius: 50%; display: inline-block; text-align: center; line-height: 40px; color: white; font-size: 18px;"">⚙</div>
                                                </td>
                                                <td style=""padding: 5px;"">
                                                    <div style=""width: 40px; height: 40px; background-color: rgba(255, 255, 255, 0.15); border-radius: 50%; display: inline-block; text-align: center; line-height: 40px; color: white; font-size: 18px;"">🌐</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Content Section -->
                    <tr>
                        <td class=""content-padding"" style=""padding: 50px 40px; background-color: #ffffff;"">
                            <!-- Title -->
                            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td style=""padding-bottom: 20px;"">
                                        <h1 style=""margin: 0; font-size: 32px; font-weight: 700; color: #1a1a1a; text-align: center; line-height: 1.2;"">
                                            Verify your email address
                                        </h1>
                                    </td>
                                </tr>
                                
                                <!-- Welcome Message -->
                                <tr>
                                    <td style=""padding-bottom: 15px; text-align: center;"">
                                        <p style=""margin: 0; font-size: 18px; color: #4a4a4a; line-height: 1.6;"">
                                            Welcome to <a href=""https://flow-tap.com"" style=""color: #4f66f1; text-decoration: none; font-weight: 600;"">flow-tap.com</a>
                                        </p>
                                    </td>
                                </tr>
                                
                                <!-- Instructions -->
                                <tr>
                                    <td style=""padding-bottom: 35px; text-align: center;"">
                                        <p style=""margin: 0; font-size: 16px; color: #666666; line-height: 1.6;"">
                                            Please click the button below to confirm your email address and activate your account.
                                        </p>
                                    </td>
                                </tr>
                                
                                <!-- CTA Button -->
                                <tr>
                                    <td align=""center"" style=""padding-bottom: 30px;"">
                                        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                            <tr>
                                                <td align=""center"" style=""border-radius: 50px; background: linear-gradient(135deg, #4f66f1 0%, #6b82f5 100%); box-shadow: 0 4px 15px rgba(79, 102, 241, 0.3);"">
                                                    <a href=""{verificationLink}"" class=""button-padding"" style=""display: inline-block; padding: 18px 50px; font-size: 16px; font-weight: 700; color: #ffffff; text-decoration: none; text-transform: uppercase; letter-spacing: 0.5px; border-radius: 50px;"">
                                                        CONFIRM EMAIL
                                                    </a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Disclaimer -->
                                <tr>
                                    <td style=""padding-bottom: 40px; text-align: center;"">
                                        <p style=""margin: 0; font-size: 13px; color: #999999; line-height: 1.5;"">
                                            If you received this in error, simply ignore this email and do not click the button.
                                        </p>
                                    </td>
                                </tr>
                                
                                <!-- Social Media Links -->
                                <tr>
                                    <td align=""center"" style=""padding-bottom: 30px;"">
                                        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                            <tr>
                                                <td style=""padding: 0 10px;"">
                                                    <a href=""https://facebook.com/flowtap"" style=""display: inline-block; width: 40px; height: 40px; background-color: #1877f2; border-radius: 50%; text-align: center; line-height: 40px; text-decoration: none;"">
                                                        <span style=""color: #ffffff; font-size: 18px; font-weight: bold;"">f</span>
                                                    </a>
                                                </td>
                                                <td style=""padding: 0 10px;"">
                                                    <a href=""https://twitter.com/flowtap"" style=""display: inline-block; width: 40px; height: 40px; background-color: #1da1f2; border-radius: 50%; text-align: center; line-height: 40px; text-decoration: none;"">
                                                        <svg width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""white"" style=""vertical-align: middle; margin-top: 10px;"">
                                                            <path d=""M23 3a10.9 10.9 0 01-3.14 1.53 4.48 4.48 0 00-7.86 3v1A10.66 10.66 0 013 4s-4 9 5 13a11.64 11.64 0 01-7 2c9 5 20 0 20-11.5a4.5 4.5 0 00-.08-.83A7.72 7.72 0 0023 3z""/>
                                                        </svg>
                                                    </a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <!-- Copyright -->
                                <tr>
                                    <td align=""center"">
                                        <p style=""margin: 0; font-size: 12px; color: #1a1a1a; line-height: 1.5;"">
                                            Copyright © {DateTime.UtcNow.Year}, flow-tap INC. All rights reserved.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
                <!-- Footer Spacing -->
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""max-width: 600px; margin-top: 20px;"">
                    <tr>
                        <td style=""text-align: center; padding: 20px;"">
                            <p style=""margin: 0; font-size: 12px; color: #999999;"">
                                This email was sent to {email}. If you have any questions, please contact us at <a href=""mailto:support@flow-tap.com"" style=""color: #4f66f1; text-decoration: none;"">support@flow-tap.com</a>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <!-- Responsive Styles -->
    <style type=""text/css"">
        @media only screen and (max-width: 600px) {{
            /* Mobile Styles */
            .mobile-padding {{
                padding: 30px 20px !important;
            }}
            
            h1 {{
                font-size: 24px !important;
            }}
            
            .header-padding {{
                padding: 40px 20px !important;
            }}
            
            .content-padding {{
                padding: 40px 25px !important;
            }}
            
            .button-padding {{
                padding: 16px 40px !important;
                font-size: 14px !important;
            }}
        }}
        
        @media only screen and (max-width: 480px) {{
            h1 {{
                font-size: 22px !important;
            }}
            
            .content-padding {{
                padding: 30px 20px !important;
            }}
        }}
        
        /* Dark mode support */
        @media (prefers-color-scheme: dark) {{
            .email-body {{
                background-color: #1a1a1a !important;
            }}
            
            .email-container {{
                background-color: #2a2a2a !important;
            }}
            
            .email-text {{
                color: #e0e0e0 !important;
            }}
        }}
    </style>
</body>
</html>";

            // Send email via SMTP
            var success = await SendEmailAsync(email, subject, body);
            
            if (success)
            {
                _logger.LogInformation("Verification email sent successfully to {Email}", email);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending verification email to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendWelcomeEmailAsync(string email, string username = "")
    {
        try
        {
            _logger.LogInformation("Sending welcome email to {Email}", email);

            var subject = "Welcome to Flowtap!";
            var body = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <style>
                        * {{
                            margin: 0;
                            padding: 0;
                            box-sizing: border-box;
                        }}
                        body {{
                            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                            line-height: 1.6;
                            color: #1a202c;
                            background: linear-gradient(135deg, #10b981 0%, #059669 50%, #34d399 100%);
                            background-attachment: fixed;
                            padding: 40px 20px;
                            min-height: 100vh;
                        }}
                        .email-wrapper {{
                            max-width: 600px;
                            margin: 0 auto;
                        }}
                        .email-container {{
                            background: #ffffff;
                            border-radius: 20px;
                            padding: 0;
                            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
                            overflow: hidden;
                        }}
                        .header-gradient {{
                            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
                            padding: 50px 40px;
                            text-align: center;
                            position: relative;
                            overflow: hidden;
                        }}
                        .header-gradient::before {{
                            content: '';
                            position: absolute;
                            top: -50%;
                            left: -50%;
                            width: 200%;
                            height: 200%;
                            background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
                            animation: pulse 4s ease-in-out infinite;
                        }}
                        @keyframes pulse {{
                            0%, 100% {{ opacity: 0.3; }}
                            50% {{ opacity: 0.6; }}
                        }}
                        .logo {{
                            position: relative;
                            z-index: 1;
                        }}
                        .logo h1 {{
                            color: #ffffff;
                            margin: 0;
                            font-size: 36px;
                            font-weight: 800;
                            letter-spacing: -1px;
                            text-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
                        }}
                        .logo-icon {{
                            font-size: 48px;
                            margin-bottom: 10px;
                            display: block;
                        }}
                        .success-badge {{
                            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
                            color: #ffffff;
                            padding: 20px;
                            text-align: center;
                            margin: 30px 40px;
                            border-radius: 12px;
                            font-weight: 700;
                            font-size: 18px;
                            box-shadow: 0 8px 20px rgba(16, 185, 129, 0.3);
                            position: relative;
                            overflow: hidden;
                        }}
                        .success-badge::before {{
                            content: '✅';
                            font-size: 24px;
                            margin-right: 10px;
                        }}
                        .content {{
                            padding: 50px 40px;
                        }}
                        .content h2 {{
                            color: #1a202c;
                            font-size: 28px;
                            font-weight: 700;
                            margin-bottom: 20px;
                            line-height: 1.2;
                        }}
                        .greeting {{
                            color: #4a5568;
                            font-size: 18px;
                            margin-bottom: 15px;
                            font-weight: 500;
                        }}
                        .greeting strong {{
                            color: #10b981;
                            font-weight: 700;
                        }}
                        .content p {{
                            color: #4a5568;
                            font-size: 16px;
                            margin-bottom: 20px;
                            line-height: 1.7;
                        }}
                        .trial-info {{
                            background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
                            border-left: 5px solid #f59e0b;
                            padding: 25px;
                            margin: 30px 0;
                            border-radius: 12px;
                            box-shadow: 0 4px 12px rgba(245, 158, 11, 0.15);
                        }}
                        .trial-info h3 {{
                            color: #92400e;
                            margin: 0 0 15px 0;
                            font-size: 20px;
                            font-weight: 700;
                        }}
                        .trial-info p {{
                            color: #78350f;
                            margin: 10px 0;
                            font-size: 15px;
                            line-height: 1.6;
                        }}
                        .trial-info p::before {{
                            content: '• ';
                            font-weight: bold;
                            margin-right: 8px;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 40px 0;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
                            color: #ffffff !important;
                            padding: 18px 48px;
                            text-decoration: none;
                            border-radius: 12px;
                            font-weight: 700;
                            font-size: 18px;
                            letter-spacing: 0.5px;
                            box-shadow: 0 8px 20px rgba(16, 185, 129, 0.4);
                            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                            position: relative;
                            overflow: hidden;
                            text-transform: uppercase;
                            border: none;
                        }}
                        .cta-button::before {{
                            content: '';
                            position: absolute;
                            top: 0;
                            left: -100%;
                            width: 100%;
                            height: 100%;
                            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
                            transition: left 0.5s;
                        }}
                        .cta-button:hover::before {{
                            left: 100%;
                        }}
                        .cta-button:hover {{
                            transform: translateY(-3px);
                            box-shadow: 0 12px 30px rgba(16, 185, 129, 0.5);
                            background: linear-gradient(135deg, #059669 0%, #10b981 100%);
                        }}
                        .cta-button:active {{
                            transform: translateY(-1px);
                        }}
                        .support-text {{
                            text-align: center;
                            color: #718096;
                            font-size: 14px;
                            margin-top: 30px;
                            padding-top: 30px;
                            border-top: 1px solid #e2e8f0;
                        }}
                        .footer {{
                            background: #f7fafc;
                            padding: 40px;
                            text-align: center;
                            border-top: 1px solid #e2e8f0;
                        }}
                        .footer p {{
                            color: #718096;
                            font-size: 14px;
                            margin: 8px 0;
                        }}
                        .footer strong {{
                            color: #4a5568;
                            font-weight: 600;
                        }}
                        .footer-copyright {{
                            margin-top: 20px;
                            font-size: 12px;
                            color: #a0aec0;
                        }}
                        @media only screen and (max-width: 600px) {{
                            .content {{
                                padding: 30px 25px;
                            }}
                            .header-gradient {{
                                padding: 40px 25px;
                            }}
                            .logo h1 {{
                                font-size: 28px;
                            }}
                            .cta-button {{
                                padding: 16px 36px;
                                font-size: 16px;
                            }}
                            .success-badge {{
                                margin: 20px 25px;
                            }}
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-wrapper'>
                        <div class='email-container'>
                            <div class='header-gradient'>
                                <div class='logo'>
                                    <span class='logo-icon'>🚀</span>
                                    <h1>Flowtap</h1>
                                </div>
                            </div>
                            
                            <div class='success-badge'>
                                Email Verified Successfully!
                            </div>
                            
                            <div class='content'>
                                <h2>Welcome to Flowtap! 🎉</h2>
                                <p class='greeting'>Hello <strong>{(string.IsNullOrEmpty(username) ? email : username)}</strong>,</p>
                                <p>Your account has been successfully verified. You're all set to start using Flowtap and explore all the amazing features we have to offer!</p>
                                
                                <div class='trial-info'>
                                    <h3>🎉 Your 30-Day Free Trial Has Started!</h3>
                                    <p>✨ Explore all our premium features</p>
                                    <p>📊 Set up your first store</p>
                                    <p>🚀 Start managing your business</p>
                                    <p>💼 Access powerful analytics and reports</p>
                                </div>
                                
                                <div class='button-container'>
                                    <a href='#' class='cta-button'>
                                        Get Started Now →
                                    </a>
                                </div>
                                
                                <p class='support-text'>If you have any questions, our support team is here to help! Just reach out anytime.</p>
                            </div>
                            
                            <div class='footer'>
                                <p>Best regards,</p>
                                <p><strong>The Flowtap Team</strong></p>
                                <p class='footer-copyright'>© {DateTime.UtcNow.Year} Flowtap. All rights reserved.</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";

            // Send email via SMTP
            var success = await SendEmailAsync(email, subject, body);
            
            if (success)
            {
                _logger.LogInformation("Welcome email sent successfully to {Email}", email);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending welcome email to {Email}", email);
            return false;
        }
    }

    /// <summary>
    /// Sends an email using SMTP
    /// </summary>
    private async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        try
        {
            // If SMTP is disabled or not configured, just log the email
            if (!_smtpEnabled || string.IsNullOrWhiteSpace(_smtpHost) || string.IsNullOrWhiteSpace(_smtpFromEmail))
            {
                _logger.LogInformation("Email (SMTP disabled): To: {ToEmail}, Subject: {Subject}", toEmail, subject);
                return true; // Return true to not break the flow, but email is not actually sent
            }

            // Create email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpFromName, _smtpFromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Create HTML body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Send email using SMTP
            using var client = new SmtpClient();
            
            try
            {
                await client.ConnectAsync(_smtpHost, _smtpPort, _smtpUseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                
                // Authenticate if credentials are provided
                if (!string.IsNullOrWhiteSpace(_smtpUsername) && !string.IsNullOrWhiteSpace(_smtpPassword))
                {
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                }
                
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
                _logger.LogInformation("Email sent successfully via SMTP to {Email}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via SMTP to {Email}. Host: {Host}, Port: {Port}", 
                    toEmail, _smtpHost, _smtpPort);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            return false;
        }
    }
}

