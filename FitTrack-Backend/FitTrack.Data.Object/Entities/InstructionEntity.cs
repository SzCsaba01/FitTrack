using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Instructions")]
public class InstructionEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "ExerciseId is required")]
    public Guid ExerciseId { get; set; }

    [Required(ErrorMessage = "Instruction is required")]
    [MaxLength(200, ErrorMessage = "Instruction cannot longer than 200 characters")]
    [MinLength(2, ErrorMessage = "Instruction cannot be shorter than 2 characters")]
    public required string Instruction { get; set; }

    [ForeignKey("ExerciseId")]
    public ExerciseEntity? Exercise { get; set; }
}
