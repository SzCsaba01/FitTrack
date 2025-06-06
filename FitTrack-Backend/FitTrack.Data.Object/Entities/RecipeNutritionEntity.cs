using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("RecipeNutritions")]
public class RecipeNutritionEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "RecipeId is required")]
    public Guid RecipeId { get; set; }

    public NutritionNameEnum Name { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public double Quantity { get; set; }

    public NutritionUnitEnum Unit { get; set; }

    [ForeignKey("RecipeId")]
    public RecipeEntity Recipe { get; set; }
}
