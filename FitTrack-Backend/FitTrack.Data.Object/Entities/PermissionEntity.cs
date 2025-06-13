using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("Permissions")]
public class PermissionEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot contain more than 100 characters")]
    public required string Name { get; set; }

    public ICollection<RolePermissionMapping> RoleMappings { get; set; }
}
