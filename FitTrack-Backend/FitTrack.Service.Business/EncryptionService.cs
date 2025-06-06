using System.Security.Cryptography;
using System.Text;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;

namespace FitTrack.Service.Business;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _pepper;

    public EncryptionService(IConfiguration configuration)
    {
        var pepper = configuration["Security:PasswordPepper"];
        if (string.IsNullOrEmpty(pepper))
        {
            throw new ConfigurationException();
        }

        _pepper = Encoding.UTF8.GetBytes(pepper);
    }

    public byte[] HashString(string input, byte[]? salt = null)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(input);
        var combined = passwordBytes.Concat(_pepper).ToArray();

        byte[] key = salt ?? _pepper;
        var hmac = new HMACSHA256(key);
        return hmac.ComputeHash(passwordBytes);
    }

    public byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public string GenerateSecureToken(int size = 32)
    {
        var bytes = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)
            .Replace('+', '_')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public bool VerifyPassword(string password, byte[] salt, byte[] hashedPassword)
    {
        var computedHash = HashString(password, salt);
        return computedHash.SequenceEqual(hashedPassword);
    }
}
