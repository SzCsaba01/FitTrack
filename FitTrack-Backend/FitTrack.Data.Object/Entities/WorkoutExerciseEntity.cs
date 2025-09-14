using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("WorkoutExercises")]
public class WorkoutExerciseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "ExerciseId is required")]
    public Guid ExerciseId { get; set; }

    [Required(ErrorMessage = "UserWorkoutId is required")]
    public Guid UserWorkoutId { get; set; }

    [ForeignKey("UserWorkoutId")]
    public WorkoutEntity? UserWorkout { get; set; }

    [ForeignKey("ExerciseId")]
    public ExerciseEntity? Exercise { get; set; }

    public ICollection<WorkoutExerciseSetEntity>? Sets { get; set; }
}
