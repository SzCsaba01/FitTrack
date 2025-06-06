using System.Net;
using System.Net.Mail;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;

namespace FitTrack.Service.Business;

public class EmailService : IEmailService
{
    private readonly string _fromMail;
    private readonly string _passKey;

    public EmailService(IConfiguration configuration)
    {
        var fromMail = configuration["EmailCredentials:Email"];
        var passKey = configuration["EmailCredentials:PassKey"];

        if (String.IsNullOrEmpty(fromMail) || String.IsNullOrEmpty(passKey))
        {
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

        await smtpClient.SendMailAsync(message);
    }

    public string CreateRegistrationEmailBody(string url, string username)
    {
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
