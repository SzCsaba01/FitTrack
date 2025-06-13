using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Requests;

public class RegistrationRequest
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public double WeightKg { get; set; }

    public double HeightCm { get; set; }

    public GenderEnum Gender { get; set; }

    public UnitSystemEnum UnitSystem { get; set; }

    public AppThemeEnum AppTheme { get; set; }
}
