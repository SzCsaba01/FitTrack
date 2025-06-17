using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Responses;

public class AuthenticationResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Permissions { get; set; }
    public UnitSystemEnum UnitSystem { get; set; }
    public AppThemeEnum AppTheme { get; set; }
}
