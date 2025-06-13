using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("Exercises")]
public class ExerciseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "EquipmentId is required")]
    public Guid EquipmentId { get; set; }

    [Required(ErrorMessage = "Difficulty is required")]
    [MaxLength(30, ErrorMessage = "Difficulty cannot be longer than 30 characters")]
    public DifficultyEnum Difficulty { get; set; }

    [MaxLength(30, ErrorMessage = "Mechanic cannot be longer than 30 characters")]
    public MechanicEnum? Mechanic { get; set; }

    [MaxLength(30, ErrorMessage = "Force cannot be longer than 30 characters")]
    public ForceEnum? Force { get; set; }

    [MaxLength(30, ErrorMessage = "Category cannot be longer than 30 characters")]
    public ExerciseCategoryEnum Category { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 characters")]
    public required string Name { get; set; }

    [MaxLength(200, ErrorMessage = "Url cannot be longer than 200 characters")]
    [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
        ErrorMessage = "ImagesUrl must be a valid URL")]
    public string? ImagesUrl { get; set; }

    [MaxLength(200, ErrorMessage = "Url cannot be longer than 200 characters")]
    [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
        ErrorMessage = "VideoUrl must be a valid URL")]
    public string? VideoUrl { get; set; }


    [ForeignKey("EquipmentId")]
    public EquipmentEntity Equipment { get; set; }

    public ICollection<InstructionEntity> Instructions { get; set; }

    public ICollection<ExercisePrimaryMuscleMapping> PrimaryMuscles { get; set; }

    public ICollection<ExerciseSecondaryMuscleMapping> SecondaryMuscles { get; set; }

    public ICollection<WorkoutExerciseEntity> Workouts { get; set; }
}
