namespace FitTrack.Domain.Entities;

public class PermissionEntity : BaseEntity
{
	public required string Name { get; set; }
	public ICollection<RolePermissionMappingEntity>? RoleMappings { get; set; }
}
