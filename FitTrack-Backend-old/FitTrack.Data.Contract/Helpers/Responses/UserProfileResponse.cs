using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Responses;
public class UserProfileResponse
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public GenderEnum Gender { get; set; }
}
