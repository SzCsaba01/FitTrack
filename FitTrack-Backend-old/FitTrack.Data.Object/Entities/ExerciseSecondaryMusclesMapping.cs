using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("ExerciseSecondaryMuscleMappings")]
public class ExerciseSecondaryMuscleMapping
{
    [Required(ErrorMessage = "ExerciseId is required")]
    public Guid ExerciseId { get; set; }

    [Required(ErrorMessage = "MuscleId is required")]
    public Guid MuscleId { get; set; }

    [ForeignKey("ExerciseId")]
    public ExerciseEntity? Exercise { get; set; }

    [ForeignKey("MuscleId")]
    public MuscleEntity? Muscle { get; set; }
}
