using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Recipes")]
public class RecipeEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 characters")]
    public required string Name { get; set; }

    public TimeSpan PreperationTime { get; set; }

    public TimeSpan CookTime { get; set; }

    public TimeSpan AdditionalTime { get; set; }

    public TimeSpan TotalTime { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Servings should be a positive number")]
    public required int Servings { get; set; }

    public ICollection<RecipeIngredientEntity>? Ingredients { get; set; }

    public ICollection<MealItemEntity>? MealItems { get; set; }

    public ICollection<RecipeDirectionEntity>? Directions { get; set; }

    public ICollection<RecipeNutritionEntity>? Nutritions { get; set; }

    public ICollection<RecipeCategoryMapping>? CategoryMappings { get; set; }
}
