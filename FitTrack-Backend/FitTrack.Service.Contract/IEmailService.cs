namespace FitTrack.Service.Contract;

public interface IEmailService
{
    public Task SendEmailAsync(string subject, string to, string body);
    public string CreateRegistrationEmailBody(string url, string username);
    public string CreateForgotPasswordEmailBody(string url, string username);
}
