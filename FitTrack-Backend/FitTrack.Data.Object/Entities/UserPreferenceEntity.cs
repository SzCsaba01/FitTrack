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

    [Required(ErrorMessage = "Weight unit is required")]
    public WeightUnitEnum WeightUnit { get; set; }

    [Required(ErrorMessage = "Distance unit is required")]
    public DistanceUnitEnum DistanceUnit { get; set; }

    [Required(ErrorMessage = "Volume unit is required")]
    public VolumeUnitEnum VolumeUnit { get; set; }

    [Required(ErrorMessage = "Application Theme is required")]
    public AppThemeEnum AppTheme { get; set; }

    [ForeignKey("UserId")]
    public UserEntity? User { get; set; }
}
