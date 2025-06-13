using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("WorkoutExerciseSets")]
public class WorkoutExerciseSetEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "WorkoutExerciseId is required")]
    public Guid WorkoutExerciseId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Set number must be positive")]
    public int SetNumber { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Repetitions must be positive")]
    public int Repetitions { get; set; }

    [Range(0.0001, double.MaxValue, ErrorMessage = "Weight must be positive")]
    public double Weight { get; set; }

    [ForeignKey("WorkoutExerciseId")]
    public WorkoutExerciseEntity WorkoutExercise { get; set; }
}
