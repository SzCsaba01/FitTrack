using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Responses;

public class AuthenticationResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<string> Permissions { get; set; }
    public UnitSystemEnum UnitSystem { get; set; }
    public AppThemeEnum AppTheme { get; set; }
}
