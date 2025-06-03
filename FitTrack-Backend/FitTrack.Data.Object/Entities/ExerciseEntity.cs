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

    [Required(ErrorMessage = "Level is required")]
    public LevelEnum Level { get; set; }

    public MechanicEnum? Mechanic { get; set; }

    public ForceEnum? Force { get; set; }

    public ExerciseCategoryEnum Category { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 characters")]
    public required string Name { get; set; }

    public string? imagesUrl { get; set; }

    public string? videoUrl { get; set; }

    [ForeignKey("EquipmentId")]
    public EquipmentEntity? Equipment { get; set; }

    public ICollection<InstructionEntity>? Instructions { get; set; }

    public ICollection<ExercisePrimaryMuscleMapping>? PrimaryMuscles { get; set; }

    public ICollection<ExerciseSecondaryMuscleMapping>? SecondaryMuscles { get; set; }
}
