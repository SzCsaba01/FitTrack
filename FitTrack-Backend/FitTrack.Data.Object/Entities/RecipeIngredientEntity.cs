using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("RecipeIngredients")]
public class RecipeIngredientEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "RecipeId is required")]
    public Guid RecipeId { get; set; }

    [Required(ErrorMessage = "IngredientId is required")]
    public Guid IngredientId { get; set; }

    public Guid? IngredientUnitId { get; set; }

    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public double? Quantity { get; set; }

    [ForeignKey("RecipeId")]
    public RecipeEntity Recipe { get; set; }

    [ForeignKey("IngredientId")]
    public IngredientEntity Ingredient { get; set; }

    [ForeignKey("IngredientUnitId")]
    public IngredientUnitEntity? Unit { get; set; }
}
