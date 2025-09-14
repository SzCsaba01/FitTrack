using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("ExerciseImages")]
public class ExerciseImageEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "ExerciseId is required")]
    public Guid ExerciseId { get; set; }

    [Required(ErrorMessage = "Image Url is requried")]
    [MaxLength(200, ErrorMessage = "Url cannot be longer than 200 characters")]
    [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
    ErrorMessage = "ImageUrl must be a valid URL")]
    public required string ImageUrl { get; set; }

    [ForeignKey("ExerciseId")]
    public ExerciseEntity? Exercise { get; set; }
}
