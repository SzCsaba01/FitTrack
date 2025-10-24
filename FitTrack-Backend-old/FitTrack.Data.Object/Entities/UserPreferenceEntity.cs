using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("UserPreferences")]
public class UserPreferenceEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    [MaxLength(30, ErrorMessage = "UnitSystem cannot be longer than 30 characters")]
    public UnitSystemEnum UnitSystem { get; set; }

    [MaxLength(30, ErrorMessage = "AppTheme cannot be longer than 30 characters")]
    public AppThemeEnum AppTheme { get; set; }

    [ForeignKey("UserId")]
    public UserEntity? User { get; set; }
}
