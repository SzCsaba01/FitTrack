using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("UserProfiles")]
public class UserProfileEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "User id is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "First name cannot be shorter than 2 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Last name cannot be shorter than 2 characters")]
    public required string LastName { get; set; }

    [MaxLength(10, ErrorMessage = "Gender cannot be longer than 100 characters")]
    public GenderEnum Gender { get; set; }

    [Required(ErrorMessage = "Date of birth is requried")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Height is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Height must be a positive number")]
    public double HeightCm { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
    public double WeightKg { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
}
