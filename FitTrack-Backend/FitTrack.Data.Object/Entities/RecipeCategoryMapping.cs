using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("RecipeCategoryMappings")]
public class RecipeCategoryMapping
{
    [Required(ErrorMessage = "RecipeId is required")]
    public Guid RecipeId { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey("RecipeId")]
    public RecipeEntity Recipe { get; set; }

    [ForeignKey("CategoryId")]
    public RecipeCategoryEntity Category { get; set; }
}

