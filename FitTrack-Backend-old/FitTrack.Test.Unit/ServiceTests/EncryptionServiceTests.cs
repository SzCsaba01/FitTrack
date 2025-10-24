using FitTrack.Service.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

public class EncryptionServiceTests
{
    private readonly string _pepper = "pepper123";
    private readonly Mock<ILogger<EncryptionService>> _loggerMock;
    private readonly EncryptionService _encryptionService;

    public EncryptionServiceTests()
    {
        _loggerMock = new Mock<ILogger<EncryptionService>>();

        var config = new Dictionary<string, string?>
        {
            {"Security:PasswordPepper", _pepper}
        };

        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        _encryptionService = new EncryptionService(configBuilder, _loggerMock.Object);
    }

    [Fact(DisplayName = "GenerateSalt generates unique random salt of correct length")]
    public void GenerateSalt_WhenCalled_ReturnsUniqueRandomSalt()
    {
        // Arrange & Act
        var salt1 = _encryptionService.GenerateSalt(16);
        var salt2 = _encryptionService.GenerateSalt(16);

        // Assert
        Assert.NotNull(salt1);
        Assert.NotNull(salt2);
        Assert.Equal(16, salt1.Length);
        Assert.Equal(16, salt2.Length);
        Assert.False(salt1.SequenceEqual(salt2));
    }

    [Fact(DisplayName = "GenerateSecureToken returns Base64 URL-safe token")]
    public void GenerateSecureToken_WhenCalled_ReturnsBase64UrlSafeString()
    {
        // Arrange & Act
        var token = _encryptionService.GenerateSecureToken(32);

        // Assert
        Assert.NotNull(token);
        Assert.DoesNotContain("+", token);
        Assert.DoesNotContain("/", token);
        Assert.DoesNotContain("=", token);
        Assert.True(token.Length > 0);
    }

    [Fact(DisplayName = "HashString returns hash without salt")]
    public void HashString_WhenNoSaltProvided_ReturnsHashedValue()
    {
        // Arrange
        var input = "password";

        // Act
        var hash = _encryptionService.HashString(input);

        // Assert
        Assert.NotNull(hash);
        Assert.True(hash.Length > 0);
    }

    [Fact(DisplayName = "HashString returns different hashes with and without salt")]
    public void HashString_WithAndWithoutSalt_ReturnsDifferentHashes()
    {
        // Arrange
        var input = "password";
        var salt = _encryptionService.GenerateSalt();

        // Act
        var hashWithSalt = _encryptionService.HashString(input, salt);
        var hashWithoutSalt = _encryptionService.HashString(input);

        // Assert
        Assert.NotNull(hashWithSalt);
        Assert.NotNull(hashWithoutSalt);
        Assert.False(hashWithSalt.SequenceEqual(hashWithoutSalt));
    }

    [Fact(DisplayName = "VerifyPassword returns true for matching password and hash")]
    public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "securePassword";
        var salt = _encryptionService.GenerateSalt();
        var hashedPassword = _encryptionService.HashString(password, salt);

        // Act
        var result = _encryptionService.VerifyPassword(password, salt, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "VerifyPassword returns false for mismatched password and hash")]
    public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var password = "securePassword";
        var salt = _encryptionService.GenerateSalt();
        var hashedPassword = _encryptionService.HashString(password, salt);

        // Act
        var result = _encryptionService.VerifyPassword("wrongPassword", salt, hashedPassword);

        // Assert
        Assert.False(result);
    }
}
