namespace FitTrack.Domain.Entities;

public class UserEntity : BaseEntity
{
    public required string Username { get; set; }
    public required byte[] HashedPassword { get; set; }
    public required byte[] Salt { get; set; }
    public byte[]? ChangePasswordToken { get; set; }
    public DateTime? ChangePasswordTokenExpiration { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public byte[]? EmailVerificationToken { get; set; }
    public DateTime? EmailVerificationExpiration { get; set; }
    public bool isEmailConfirmed { get; set; }
    public byte[]? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public Guid RoleId { get; set; }
    public RoleEntity? Role { get; set; }
    public UserProfileEntity? UserProfile { get; set; }
    public UserPreferenceEntity? UserPreference { get; set; }
}
