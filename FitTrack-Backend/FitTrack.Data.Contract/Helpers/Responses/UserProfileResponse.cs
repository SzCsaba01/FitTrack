using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Responses;
public class UserProfileResponse
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public GenderEnum Gender { get; set; }
}
