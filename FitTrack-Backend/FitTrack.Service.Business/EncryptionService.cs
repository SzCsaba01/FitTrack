using System.Security.Cryptography;
using System.Text;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;

public class EncryptionService : IEncryptionService
{
    private readonly ILogger<EncryptionService> _logger;
    private readonly byte[] _pepper;

    public EncryptionService(IConfiguration configuration, ILogger<EncryptionService> logger)
    {
        _logger = logger;

        var pepper = configuration["Security:PasswordPepper"];
        if (string.IsNullOrEmpty(pepper))
        {
            _logger.LogCritical("Password pepper is missing from configuration.");
            throw new ConfigurationException();
        }

        _pepper = Encoding.UTF8.GetBytes(pepper);
    }

    public byte[] HashString(string input, byte[]? salt = null)
    {
        _logger.LogDebug("HashString called. Input length: {Length}, Salt provided: {HasSalt}", input.Length, salt != null);

        var passwordBytes = Encoding.UTF8.GetBytes(input);
        var combined = passwordBytes.Concat(_pepper).ToArray();

        byte[] key = salt ?? _pepper;
        var hmac = new HMACSHA256(key);

        var hash = hmac.ComputeHash(passwordBytes);

        _logger.LogDebug("HashString produced hash of length {HashLength}", hash.Length);

        return hash;
    }

    public byte[] GenerateSalt(int size = 16)
    {
        _logger.LogDebug("GenerateSalt called. Size: {Size}", size);

        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        _logger.LogInformation("Generated new salt of length {Size}", size);

        return salt;
    }

    public string GenerateSecureToken(int size = 32)
    {
        _logger.LogDebug("GenerateSecureToken called. Size: {Size}", size);

        var bytes = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        _logger.LogInformation("Generated secure token of size {Size}", size);

        return Convert.ToBase64String(bytes)
            .Replace('+', '_')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public bool VerifyPassword(string password, byte[] salt, byte[] hashedPassword)
    {
        _logger.LogDebug("VerifyPassword called. Password length: {Length}, Salt length: {SaltLength}", password.Length, salt.Length);

        var computedHash = HashString(password, salt);

        var isMatch = computedHash.SequenceEqual(hashedPassword);

        _logger.LogInformation("Password verification result: {Result}", isMatch);

        return isMatch;
    }
}
