using MealTimes.Core.Service;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MealTimes.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink, string userName)
        {
            var subject = "Password Reset Request - MealTimes";
            var body = GeneratePasswordResetEmailBody(resetLink, userName);
            
            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@mealtimes.com";
                var fromPassword = _configuration["EmailSettings:FromPassword"] ?? "";
                var fromName = _configuration["EmailSettings:FromName"] ?? "MealTimes";

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a proper logging framework)
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        private string GeneratePasswordResetEmailBody(string resetLink, string userName)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Password Reset - MealTimes</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #f8f9fa; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }}
                        .content {{ background-color: #ffffff; padding: 30px; border: 1px solid #dee2e6; }}
                        .footer {{ background-color: #f8f9fa; padding: 15px; text-align: center; border-radius: 0 0 8px 8px; font-size: 12px; color: #6c757d; }}
                        .btn {{ display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        .btn:hover {{ background-color: #0056b3; }}
                        .warning {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1 style='margin: 0; color: #007bff;'>MealTimes</h1>
                            <p style='margin: 5px 0 0 0;'>Password Reset Request</p>
                        </div>
                        
                        <div class='content'>
                            <h2>Hello {userName},</h2>
                            
                            <p>We received a request to reset your password for your MealTimes account. If you made this request, please click the button below to reset your password:</p>
                            
                            <div style='text-align: center;'>
                                <a href='{resetLink}' class='btn'>Reset Your Password</a>
                            </div>
                            
                            <p>Or copy and paste this link into your browser:</p>
                            <p style='word-break: break-all; background-color: #f8f9fa; padding: 10px; border-radius: 5px;'>{resetLink}</p>
                            
                            <div class='warning'>
                                <strong>Important:</strong>
                                <ul>
                                    <li>This link will expire in 5 minutes for security reasons</li>
                                    <li>If you didn't request this password reset, please ignore this email</li>
                                    <li>Your password will remain unchanged until you create a new one</li>
                                </ul>
                            </div>
                            
                            <p>If you're having trouble clicking the button, you can also reset your password by visiting our website and using the 'Forgot Password' feature.</p>
                            
                            <p>Best regards,<br>The MealTimes Team</p>
                        </div>
                        
                        <div class='footer'>
                            <p>This is an automated message. Please do not reply to this email.</p>
                            <p>&copy; 2025 MealTimes. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}