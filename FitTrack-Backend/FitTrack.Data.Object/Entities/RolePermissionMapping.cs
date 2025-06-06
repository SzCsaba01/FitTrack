using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitTrack.Data.Object.Entities;

[Table("RolePermissionMappings")]
public class RolePermissionMapping
{
    [Required(ErrorMessage = "RoleId is required")]
    public Guid RoleId { get; set; }

    [Required(ErrorMessage = "PermissionId is required")]
    public Guid PermissionId { get; set; }

    [ForeignKey("RoleId")]
    public RoleEntity Role { get; set; }

    [ForeignKey("PermissionId")]
    public PermissionEntity Permission { get; set; }
}
