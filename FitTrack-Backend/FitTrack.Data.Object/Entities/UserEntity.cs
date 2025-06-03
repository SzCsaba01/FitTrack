using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Users")]
public class UserEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(30, ErrorMessage = "Username cannot be longer than 30 characters")]
    [MinLength(5, ErrorMessage = "Username cannot be shorter than 5 characters")]
    [RegularExpression("[a-zA-Z0-9._]+", ErrorMessage = "Username can contain only lower and upper case characters and numbers")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    public string? ResetPasswordToken { get; set; }

    public DateTime? ResetPasswordTokenExpirationDate { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [MaxLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Registration date is required")]
    public DateTime RegistrationDate { get; set; }

    public string? RegistrationToken { get; set; }

    [Required(ErrorMessage = "Is email confirmed is required")]
    public bool isEmailConfirmed { get; set; }

    public string? RefreshToken { get; set; }

    [Required(ErrorMessage = "Role id is required")]
    public Guid RoleId { get; set; }

    [ForeignKey("RoleId")]
    public RoleEntity? Role { get; set; }

    public UserProfileEntity? UserProfile { get; set; }
}
