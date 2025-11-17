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

            var subject = "Verify Your Email - Flowtap";
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
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
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
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
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
                            color: #667eea;
                            font-weight: 700;
                        }}
                        .content p {{
                            color: #4a5568;
                            font-size: 16px;
                            margin-bottom: 20px;
                            line-height: 1.7;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 50px 0;
                            padding: 20px 0;
                        }}
                        .verify-button {{
                            display: inline-block;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
                            color: #ffffff !important;
                            padding: 24px 64px;
                            text-decoration: none;
                            border-radius: 16px;
                            font-weight: 800;
                            font-size: 22px;
                            letter-spacing: 1px;
                            box-shadow: 0 12px 40px rgba(102, 126, 234, 0.5), 
                                        0 4px 12px rgba(118, 75, 162, 0.3),
                                        inset 0 1px 0 rgba(255, 255, 255, 0.2);
                            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
                            position: relative;
                            overflow: hidden;
                            text-transform: uppercase;
                            border: 2px solid rgba(255, 255, 255, 0.2);
                            min-width: 320px;
                            text-align: center;
                            line-height: 1.5;
                            cursor: pointer;
                            font-family: inherit;
                        }}
                        .verify-button::before {{
                            content: '';
                            position: absolute;
                            top: 0;
                            left: -100%;
                            width: 100%;
                            height: 100%;
                            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.4), transparent);
                            transition: left 0.6s ease-in-out;
                        }}
                        .verify-button:hover::before {{
                            left: 100%;
                        }}
                        .verify-button:hover {{
                            transform: translateY(-5px) scale(1.02);
                            box-shadow: 0 20px 50px rgba(102, 126, 234, 0.6), 
                                        0 8px 20px rgba(118, 75, 162, 0.4),
                                        inset 0 1px 0 rgba(255, 255, 255, 0.3);
                            background: linear-gradient(135deg, #764ba2 0%, #667eea 50%, #764ba2 100%);
                        }}
                        .verify-button:active {{
                            transform: translateY(-2px) scale(0.98);
                            box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
                        }}
                        .button-icon {{
                            margin-right: 12px;
                            font-size: 28px;
                            vertical-align: middle;
                            display: inline-block;
                            animation: sparkle 2s ease-in-out infinite;
                        }}
                        @keyframes sparkle {{
                            0%, 100% {{ transform: scale(1) rotate(0deg); opacity: 1; }}
                            50% {{ transform: scale(1.2) rotate(180deg); opacity: 0.8; }}
                        }}
                        .link-alternative {{
                            text-align: center;
                            margin: 30px 0;
                        }}
                        .link-alternative p {{
                            color: #718096;
                            font-size: 14px;
                            margin-bottom: 15px;
                            font-weight: 500;
                        }}
                        .link-text {{
                            background: linear-gradient(135deg, #f7fafc 0%, #edf2f7 100%);
                            padding: 16px;
                            border-radius: 10px;
                            word-break: break-all;
                            font-size: 13px;
                            color: #4a5568;
                            font-family: 'Courier New', 'Monaco', monospace;
                            border: 1px solid #e2e8f0;
                            box-shadow: inset 0 2px 4px rgba(0,0,0,0.06);
                        }}
                        .warning-box {{
                            background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
                            border-left: 5px solid #f59e0b;
                            padding: 20px;
                            margin: 30px 0;
                            border-radius: 12px;
                            box-shadow: 0 4px 12px rgba(245, 158, 11, 0.15);
                        }}
                        .warning-box strong {{
                            color: #92400e;
                            font-size: 16px;
                            display: block;
                            margin-bottom: 8px;
                        }}
                        .warning-box p {{
                            color: #78350f;
                            margin: 0;
                            font-size: 14px;
                            line-height: 1.6;
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
                            .verify-button {{
                                padding: 20px 48px;
                                font-size: 18px;
                                min-width: 280px;
                            }}
                            .button-icon {{
                                font-size: 24px;
                                margin-right: 10px;
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
                            <div class='content'>
                                <h2>Welcome to Flowtap! 👋</h2>
                                <p class='greeting'>Hello <strong>{(string.IsNullOrEmpty(username) ? email : username)}</strong>,</p>
                                <p>Thank you for joining Flowtap! We're thrilled to have you on board. To get started and unlock all features, please verify your email address by clicking the button below.</p>
                                
                                <div class='button-container'>
                                    <a href='{verificationLink}' 
                                       class='verify-button' 
                                       style='display: inline-block; text-decoration: none; cursor: pointer;'>
                                        <span class='button-icon'>✨</span>
                                        Verify Email Address
                                    </a>
                                </div>
                                
                                <div class='link-alternative'>
                                    <p>Or copy and paste this link into your browser:</p>
                                    <div class='link-text'>{verificationLink}</div>
                                </div>
                                
                                <div class='warning-box'>
                                    <strong>⏰ Important Notice</strong>
                                    <p>This verification link will expire in 24 hours. Please verify your email as soon as possible to ensure uninterrupted access to your account.</p>
                                </div>
                                
                                <p style='color: #718096; font-size: 14px; text-align: center; margin-top: 30px;'>If you didn't create an account with Flowtap, you can safely ignore this email.</p>
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

