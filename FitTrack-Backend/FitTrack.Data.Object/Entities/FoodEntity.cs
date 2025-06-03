using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Foods")]
public class FoodEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "CategoryId is required")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 characters")]
    public required string Name { get; set; }

    [Range(0.0001, int.MaxValue, ErrorMessage = "Quantity should be a positive number")]
    public int QuantityG { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Calories should be a positive number")]
    public int Calories { get; set; }

    [ForeignKey("CategoryId")]
    public FoodCategoryEntity? Category { get; set; }

    public ICollection<FoodNutritionEntity>? Nutritions { get; set; }

    public ICollection<MealItemEntity>? MealItems { get; set; }
}
