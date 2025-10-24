using FitTrack.Common.Enums;

namespace FitTrack.Domain.Entities;

public class UserProfileEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public double HeightCm { get; set; }
    public double WeightKg { get; set; }
    public UserEntity? User { get; set; }
}
