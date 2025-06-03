using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("MealItems")]
public class MealItemEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "MealId is required")]
    public Guid MealId { get; set; }

    public Guid? FoodId { get; set; }

    public Guid? RecipeId { get; set; }

    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be positive")]
    public double Quantity { get; set; }

    [ForeignKey("MealId")]
    public UserMealEntity? Meal { get; set; }

    [ForeignKey("FoodId")]
    public FoodEntity? Food { get; set; }

    [ForeignKey("RecipeId")]
    public RecipeEntity? Recipe { get; set; }
}
