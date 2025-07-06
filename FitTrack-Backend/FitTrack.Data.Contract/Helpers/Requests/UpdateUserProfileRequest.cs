using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Requests;
public class UpdateUserProfileRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    public GenderEnum Gender { get; set; }
}
