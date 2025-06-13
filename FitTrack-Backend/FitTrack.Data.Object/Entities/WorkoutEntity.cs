using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Workouts")]
public class WorkoutEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }

    public ICollection<WorkoutExerciseEntity> WorkoutExercises { get; set; }

    public ICollection<WorkoutCardioExerciseEntity> WorkoutCardioExercises { get; set; }
}
