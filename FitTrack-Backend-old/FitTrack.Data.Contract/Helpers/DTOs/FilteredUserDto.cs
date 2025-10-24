using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract.Helpers.DTOs;

public class FilteredUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string ProfilePictureUrl { get; set; }
    public required DateTime RegistrationDate { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public RoleEnum Role { get; set; }
}
