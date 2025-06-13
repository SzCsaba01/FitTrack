using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Object.Entities;

[Table("Roles")]
public class RoleEntity
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(30, ErrorMessage = "RoleName cannot be longer than 30 characters")]
    public RoleEnum RoleName { get; set; }

    public ICollection<RolePermissionMapping> PermissionMappings { get; set; }

    public ICollection<UserEntity> Users { get; set; }
}
