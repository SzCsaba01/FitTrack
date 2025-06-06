using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FitTrack.Service.Business;

public class JwtService : IJwtService
{
    private readonly byte[] _encryptionKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(IConfiguration configuration)
    {
        var encryptionKey = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        if (String.IsNullOrEmpty(encryptionKey) || String.IsNullOrEmpty(issuer) || String.IsNullOrEmpty(audience))
        {
            throw new ConfigurationException();
        }

        _encryptionKey = Encoding.UTF8.GetBytes(encryptionKey);
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateJwt(ClaimsIdentity claims, DateTime expirationDate)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(_encryptionKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expirationDate,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Audience = _audience,
            Issuer = _issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}
