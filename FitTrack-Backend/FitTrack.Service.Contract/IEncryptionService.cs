namespace FitTrack.Service.Contract;

public interface IEncryptionService
{
    public byte[] HashString(string password, byte[]? salt = null);
    public byte[] GenerateSalt(int size = 16);
    public string GenerateSecureToken(int size = 32);
    bool VerifyPassword(string password, byte[] salt, byte[] hashedPassword);
}
