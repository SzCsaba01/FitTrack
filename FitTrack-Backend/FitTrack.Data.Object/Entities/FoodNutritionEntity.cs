using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("FoodNutritions")]
public class FoodNutritionEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "FoodId is required")]
    public Guid FoodId { get; set; }

    [MaxLength(30, ErrorMessage = "Name cannot be longer than 30 characters")]
    public NutritionNameEnum Name { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public double Quantity { get; set; }

    [MaxLength(10, ErrorMessage = "Unit cannot be longer than 10 characters")]
    public NutritionUnitEnum Unit { get; set; }

    public bool LessThan { get; set; }

    public bool MoreThan { get; set; }

    [ForeignKey("FoodId")]
    public FoodEntity Food { get; set; }
}
