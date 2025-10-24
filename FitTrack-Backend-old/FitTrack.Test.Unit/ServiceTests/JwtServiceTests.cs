using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FitTrack.Service.Business;
using Microsoft.Extensions.Configuration;

namespace FitTrack.Test.Unit.ServiceTests;

public class JwtServiceTests
{
    private readonly string _key = "SuperSecretTestKey1234567890123456";
    private readonly string _issuer = "TestIssuer";
    private readonly string _audience = "TestAudience";

    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {

        var config = new Dictionary<string, string?>
        {
            { "Jwt:Key", _key },
            { "Jwt:Issuer", _issuer },
            { "Jwt:Audience", _audience }
        };

        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        _jwtService = new JwtService(configBuilder);
    }


    [Fact(DisplayName = "GenerateJwt returns a valid JWT with correct claims and issuer/audience")]
    public void GenerateJwt_WithValidClaims_ReturnsValidJwtTokenContainingClaimsIssuerAndAudience()
    {
        // Arrange
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Role, "Admin")
        });

        var expires = DateTime.UtcNow.AddMinutes(15);

        // Act
        var token = _jwtService.GenerateJwt(claims, expires);

        // Assert
        Assert.False(string.IsNullOrEmpty(token));

        var handler = new JwtSecurityTokenHandler();
        var parsed = handler.ReadJwtToken(token);

        Assert.Equal(_issuer, parsed.Issuer);
        Assert.Equal(_audience, parsed.Audiences.First());
        Assert.Contains(parsed.Claims, c => c.Type == "nameid" && c.Value == "123");
        Assert.Contains(parsed.Claims, c => c.Type == "role" && c.Value == "Admin");
    }

    [Fact(DisplayName = "GenerateJwt sets token expiration correctly")]
    public void GenerateJwt_WithExpiration_SetsTokenExpirationCorrectly()
    {
        // Arrange
        var claims = new ClaimsIdentity();
        var expectedExpiry = DateTime.UtcNow.AddMinutes(5);

        // Act
        var token = _jwtService.GenerateJwt(claims, expectedExpiry);
        var handler = new JwtSecurityTokenHandler();
        var parsed = handler.ReadJwtToken(token);

        // Assert
        Assert.True(Math.Abs((parsed.ValidTo - expectedExpiry).TotalSeconds) < 5); // Allow small clock skew
    }
}
