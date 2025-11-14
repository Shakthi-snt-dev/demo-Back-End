using Flowtap_Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
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
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                            background-color: #f5f5f5;
                        }}
                        .email-container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 40px;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        }}
                        .logo {{
                            text-align: center;
                            margin-bottom: 30px;
                        }}
                        .logo h1 {{
                            color: #2563eb;
                            margin: 0;
                            font-size: 28px;
                            font-weight: 700;
                        }}
                        .content {{
                            margin-bottom: 30px;
                        }}
                        .content h2 {{
                            color: #1f2937;
                            font-size: 24px;
                            margin-bottom: 20px;
                        }}
                        .content p {{
                            color: #4b5563;
                            font-size: 16px;
                            margin-bottom: 15px;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 30px 0;
                        }}
                        .verify-button {{
                            display: inline-block;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            color: #ffffff !important;
                            padding: 14px 32px;
                            text-decoration: none;
                            border-radius: 8px;
                            font-weight: 600;
                            font-size: 16px;
                            box-shadow: 0 4px 6px rgba(102, 126, 234, 0.3);
                            transition: all 0.3s ease;
                            letter-spacing: 0.5px;
                        }}
                        .verify-button:hover {{
                            background: linear-gradient(135deg, #764ba2 0%, #667eea 100%);
                            box-shadow: 0 6px 12px rgba(102, 126, 234, 0.4);
                            transform: translateY(-2px);
                        }}
                        .link-text {{
                            background-color: #f3f4f6;
                            padding: 12px;
                            border-radius: 6px;
                            word-break: break-all;
                            font-size: 12px;
                            color: #6b7280;
                            margin-top: 20px;
                            font-family: 'Courier New', monospace;
                        }}
                        .footer {{
                            margin-top: 40px;
                            padding-top: 20px;
                            border-top: 1px solid #e5e7eb;
                            text-align: center;
                            color: #9ca3af;
                            font-size: 14px;
                        }}
                        .warning {{
                            background-color: #fef3c7;
                            border-left: 4px solid #f59e0b;
                            padding: 12px;
                            margin: 20px 0;
                            border-radius: 4px;
                            font-size: 14px;
                            color: #92400e;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='logo'>
                            <h1>🚀 Flowtap</h1>
                        </div>
                        <div class='content'>
                            <h2>Welcome to Flowtap!</h2>
                            <p>Hello <strong>{(string.IsNullOrEmpty(username) ? email : username)}</strong>,</p>
                            <p>Thank you for registering with Flowtap! We're excited to have you on board. To get started, please verify your email address by clicking the button below:</p>
                            
                            <div class='button-container'>
                                <a href='{verificationLink}' class='verify-button'>✨ Verify Email Address</a>
                            </div>
                            
                            <p style='text-align: center; color: #6b7280; font-size: 14px;'>Or copy and paste this link into your browser:</p>
                            <div class='link-text'>{verificationLink}</div>
                            
                            <div class='warning'>
                                <strong>⏰ Important:</strong> This verification link will expire in 24 hours. Please verify your email as soon as possible.
                            </div>
                            
                            <p style='color: #6b7280; font-size: 14px;'>If you didn't create an account with Flowtap, please ignore this email.</p>
                        </div>
                        
                        <div class='footer'>
                            <p>Best regards,<br/><strong>The Flowtap Team</strong></p>
                            <p style='margin-top: 20px; font-size: 12px;'>© {DateTime.UtcNow.Year} Flowtap. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            // Simulate email sending
            await Task.Delay(100);

            _logger.LogInformation("Verification email sent successfully to {Email}", email);
            return true;
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
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                            background-color: #f5f5f5;
                        }}
                        .email-container {{
                            background-color: #ffffff;
                            border-radius: 8px;
                            padding: 40px;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        }}
                        .logo {{
                            text-align: center;
                            margin-bottom: 30px;
                        }}
                        .logo h1 {{
                            color: #2563eb;
                            margin: 0;
                            font-size: 28px;
                            font-weight: 700;
                        }}
                        .success-badge {{
                            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
                            color: white;
                            padding: 12px 24px;
                            border-radius: 8px;
                            text-align: center;
                            margin: 20px 0;
                            font-weight: 600;
                            font-size: 16px;
                        }}
                        .content {{
                            margin-bottom: 30px;
                        }}
                        .content h2 {{
                            color: #1f2937;
                            font-size: 24px;
                            margin-bottom: 20px;
                        }}
                        .content p {{
                            color: #4b5563;
                            font-size: 16px;
                            margin-bottom: 15px;
                        }}
                        .trial-info {{
                            background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
                            border-left: 4px solid #f59e0b;
                            padding: 20px;
                            margin: 25px 0;
                            border-radius: 8px;
                        }}
                        .trial-info h3 {{
                            color: #92400e;
                            margin: 0 0 10px 0;
                            font-size: 18px;
                        }}
                        .trial-info p {{
                            color: #78350f;
                            margin: 5px 0;
                            font-size: 14px;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            color: #ffffff !important;
                            padding: 14px 32px;
                            text-decoration: none;
                            border-radius: 8px;
                            font-weight: 600;
                            font-size: 16px;
                            box-shadow: 0 4px 6px rgba(102, 126, 234, 0.3);
                            margin: 20px 0;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 30px 0;
                        }}
                        .footer {{
                            margin-top: 40px;
                            padding-top: 20px;
                            border-top: 1px solid #e5e7eb;
                            text-align: center;
                            color: #9ca3af;
                            font-size: 14px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='logo'>
                            <h1>🚀 Flowtap</h1>
                        </div>
                        
                        <div class='success-badge'>
                            ✅ Email Verified Successfully!
                        </div>
                        
                        <div class='content'>
                            <h2>Welcome to Flowtap, {(string.IsNullOrEmpty(username) ? email : username)}!</h2>
                            <p>Your account has been successfully verified. You're all set to start using Flowtap!</p>
                            
                            <div class='trial-info'>
                                <h3>🎉 Your 30-Day Free Trial Has Started!</h3>
                                <p>✨ Explore all our premium features</p>
                                <p>📊 Set up your first store</p>
                                <p>🚀 Start managing your business</p>
                            </div>
                            
                            <div class='button-container'>
                                <a href='#' class='cta-button'>Get Started Now →</a>
                            </div>
                            
                            <p style='color: #6b7280; font-size: 14px;'>If you have any questions, our support team is here to help!</p>
                        </div>
                        
                        <div class='footer'>
                            <p>Best regards,<br/><strong>The Flowtap Team</strong></p>
                            <p style='margin-top: 20px; font-size: 12px;'>© {DateTime.UtcNow.Year} Flowtap. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            // Simulate email sending
            await Task.Delay(100);

            _logger.LogInformation("Welcome email sent successfully to {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending welcome email to {Email}", email);
            return false;
        }
    }
}

