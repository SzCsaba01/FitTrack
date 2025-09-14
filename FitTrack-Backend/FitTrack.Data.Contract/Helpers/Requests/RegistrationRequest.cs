using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Requests;

public class RegistrationRequest
{
    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public double WeightKg { get; set; }

    public double HeightCm { get; set; }

    public GenderEnum Gender { get; set; }

    public UnitSystemEnum UnitSystem { get; set; }

    public AppThemeEnum AppTheme { get; set; }
}
