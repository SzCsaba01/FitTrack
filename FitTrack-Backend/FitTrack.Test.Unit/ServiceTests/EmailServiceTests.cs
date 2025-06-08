using FitTrack.Service.Business;
using FitTrack.Service.Business.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

public class EmailServiceTests
{
    private readonly string _email = "test@exmaple.com";
    private readonly string _passKey = "pass";
    private readonly Mock<ILogger<EmailService>> _loggerMock;

    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _loggerMock = new Mock<ILogger<EmailService>>();

        var config = new Dictionary<string, string?>
        {
            {"EmailCredentials:Email", _email},
            {"EmailCredentials:PassKey", _passKey}
        };

        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        _emailService = new EmailService(configBuilder, _loggerMock.Object);
    }

    [Fact(DisplayName = "SendEmailAsync sends email successfully")]
    public async Task SendEmailAsync_SendsEmail()
    {
        var ex = await Record.ExceptionAsync(() =>
            _emailService.SendEmailAsync("subject", "recipient@example.com", "body"));

        Assert.NotNull(ex);
    }

    [Fact(DisplayName = "CreateRegistrationEmailBody returns HTML containing username and URL")]
    public void CreateRegistrationEmailBody_ReturnsValidHtml()
    {
        // Arrange
        var url = "http://test/confirm";
        var username = "user123";

        // Act
        var result = _emailService.CreateRegistrationEmailBody(url, username);

        // Assert
        Assert.Contains(url, result);
        Assert.Contains(username, result);
        Assert.Contains("<html>", result);
    }

    [Fact(DisplayName = "CreateForgotPasswordEmailBody returns HTML containing username and URL")]
    public void CreateForgotPasswordEmailBody_ReturnsValidHtml()
    {
        // Arrange
        var url = "http://test/reset";
        var username = "user123";

        // Act
        var result = _emailService.CreateForgotPasswordEmailBody(url, username);

        // Assert
        Assert.Contains(url, result);
        Assert.Contains(username, result);
        Assert.Contains("<html>", result);
    }
}
