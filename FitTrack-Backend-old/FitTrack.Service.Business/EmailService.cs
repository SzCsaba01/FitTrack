using System.Net;
using System.Net.Mail;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;

public class EmailService : IEmailService
{
    private readonly string _fromMail;
    private readonly string _passKey;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;

        var fromMail = configuration["EmailCredentials:Email"];
        var passKey = configuration["EmailCredentials:PassKey"];

        if (String.IsNullOrEmpty(fromMail) || String.IsNullOrEmpty(passKey))
        {
            _logger.LogCritical("Email credentials missing from configuration.");
            throw new ConfigurationException();
        }

        _fromMail = fromMail;
        _passKey = passKey;
    }

    public async Task SendEmailAsync(string subject, string to, string body)
    {
        MailMessage message = new MailMessage();

        message.From = new MailAddress(_fromMail);
        message.Subject = subject;
        message.To.Add(new MailAddress(to));
        message.Body = body;
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(_fromMail, _passKey),
            EnableSsl = true,
        };

        try
        {
            _logger.LogInformation("Attempting to send email to {Recipient} with subject '{Subject}'", to, subject);
            await smtpClient.SendMailAsync(message);
            _logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}. Subject: {Subject}", to, subject);
            throw;
        }
    }

    public string CreateRegistrationEmailBody(string url, string username)
    {
        _logger.LogDebug("Creating registration email body for user: {Username}", username);
        return $@"
			<html>
			<head>
				<style>
					html {{
						font-size: 1rem;
					}}
					body {{
						font-family: Arial, sans-serif;
						background-color: #f9f9f9;
						padding: 1.25rem;
					}}
					.container {{
						max-width: 37.5rem; 
						margin: auto;
						background-color: #ffffff;
						padding: 2rem;
						border-radius: 0.5rem;
						box-shadow: 0 0.125rem 0.25rem rgba(0,0,0,0.1);
					}}
					.button {{
						display: inline-block;
						margin-top: 1.25rem;
						padding: 0.75rem 1.5rem;
						background-color: #4CAF50;
						color: white;
						text-decoration: none;
						border-radius: 0.3125rem;
						font-size: 1rem;
					}}
				</style>
			</head>
			<body>
				<div class='container'>
					<h2>Welcome to FitTrack, {username}!</h2>
					<p>Thank you for registering. To complete your registration and activate your account, please click the button below:</p>
					<a href='{url}' class='button'>Confirm Registration</a>
					<p>If you did not request this, please ignore this email.</p>
					<br />
					<p>Best regards,<br/>The FitTrack Team</p>
				</div>
			</body>
			</html>";
    }

    public string CreateForgotPasswordEmailBody(string url, string username)
    {
        _logger.LogDebug("Creating forgot password email body for user: {Username}", username);
        return $@"
        <html>
        <head>
            <style>
                html {{
                    font-size: 16px;
                }}
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 1.25rem;
                    margin: 0;
                }}
                .container {{
                    max-width: 37.5rem;
                    margin: auto;
                    background-color: #ffffff;
                    padding: 2rem;
                    border-radius: 0.5rem;
                    box-shadow: 0 0.125rem 0.25rem rgba(0,0,0,0.1);
                }}
                .button {{
                    display: inline-block;
                    margin-top: 1.25rem;
                    padding: 0.75rem 1.5rem;
                    background-color: #007bff;
                    color: #ffffff;
                    text-decoration: none;
                    border-radius: 0.3125rem;
                    font-size: 1rem;
                }}
                .footer {{
                    margin-top: 2rem;
                    font-size: 0.875rem;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Password Reset Requested</h2>
                <p>Hello {username},</p>
                <p>We received a request to reset your password. Click the button below to proceed:</p>
                <a href='{url}' class='button'>Reset Password</a>
                <p>If you did not request a password reset, please ignore this email. Your password will remain unchanged.</p>
                <div class='footer'>
                    <p>— The FitTrack Team</p>
                </div>
            </div>
        </body>
        </html>";
    }
}
