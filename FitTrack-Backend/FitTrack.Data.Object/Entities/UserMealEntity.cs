using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("UserMeals")]
public class UserMealEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    public DateTime Date { get; set; }

    public MealTypeEnum Type { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }

    public ICollection<MealItemEntity> MealItems { get; set; }
}
