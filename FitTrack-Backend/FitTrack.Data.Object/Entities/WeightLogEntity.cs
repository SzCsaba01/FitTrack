using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("WeightLogs")]
public class WeightLogEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(1, double.MaxValue, ErrorMessage = "Weight must be at least 1")]
    public double WeightKg { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [MaxLength(255, ErrorMessage = "Notes cannot be more than 255 characters long")]
    public string? Note { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
}
