using FitTrack.Common.Enums;

namespace FitTrack.Domain.Entities;

public class RoleEntity : BaseEntity
{
	public RoleEnum RoleName { get; set; }
	public IEnumerable<RolePermissionMappingEntity>? PermissionMappings { get; set; }
	public IEnumerable<UserEntity>? Users;
}
