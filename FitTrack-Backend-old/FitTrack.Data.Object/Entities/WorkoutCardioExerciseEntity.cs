using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("WorkoutCardioExercise")]
public class WorkoutCardioExerciseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "ExerciseId is required")]
    public Guid ExerciseId { get; set; }

    [Required(ErrorMessage = "UserWorkoutId is required")]
    public Guid UserWorkoutId { get; set; }

    public int CardioNumber { get; set; }

    public TimeSpan Duration { get; set; }

    public double? Distance { get; set; }

    public double? CaloriesBurned { get; set; }

    public double? AverageHeartRate { get; set; }

    [ForeignKey("UserWorkoutId")]
    public WorkoutEntity? UserWorkout { get; set; }

    [ForeignKey("ExerciseId")]
    public ExerciseEntity? Exercise { get; set; }
}
