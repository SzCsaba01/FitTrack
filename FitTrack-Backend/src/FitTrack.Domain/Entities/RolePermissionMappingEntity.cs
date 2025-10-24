namespace FitTrack.Domain.Entities;

public class RolePermissionMappingEntity
{
	public int RoleId { get; set; }
	public int PermissionId { get; set; }
	public RoleEntity? Role { get; set; }
	public PermissionEntity? Permission { get; set; }
}
