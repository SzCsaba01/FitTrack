using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Data.Contract;

public interface IRoleRepository
{
    public Task<RoleEntity?> GetRoleByNameAsync(RoleEnum roleName);
}
