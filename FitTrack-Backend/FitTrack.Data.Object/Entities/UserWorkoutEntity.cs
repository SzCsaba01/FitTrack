using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("UserWorkouts")]
public class UserWorkoutEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }

    public ICollection<WorkoutExerciseSetEntity> WorkoutExerciseSets { get; set; }

    public ICollection<WorkoutCardioExerciseEntity> WorkoutCardioExercises { get; set; }
}
