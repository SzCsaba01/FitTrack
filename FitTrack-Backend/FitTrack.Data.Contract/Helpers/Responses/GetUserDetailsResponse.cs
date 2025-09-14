using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.Responses;

public class GetUserDetailsResponse
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime RegistrationDate { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public RoleEnum Role { get; set; }
    public GenderEnum Gender { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
}
