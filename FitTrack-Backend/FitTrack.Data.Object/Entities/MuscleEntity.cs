using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Muscles")]
public class MuscleEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 characters")]
    public required string Name { get; set; }

    public ICollection<ExercisePrimaryMuscleMapping>? PrimaryExercises { get; set; }

    public ICollection<ExerciseSecondaryMuscleMapping>? SecondaryExercises { get; set; }
}
