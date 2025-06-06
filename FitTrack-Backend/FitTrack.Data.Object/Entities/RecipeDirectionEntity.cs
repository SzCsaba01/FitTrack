using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("RecipeDirections")]
public class RecipeDirectionEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "RecipeId is required")]
    public Guid RecipeId { get; set; }

    [Required(ErrorMessage = "Direction is required")]
    [MaxLength(200, ErrorMessage = "Direction cannot be longer than 200 characters")]
    [MinLength(10, ErrorMessage = "Direction cannot be shorter than 10 characters")]
    public required string Direction { get; set; }

    [ForeignKey("RecipeId")]
    public RecipeEntity Recipe { get; set; }
}
