using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;

public class RoleRepository : IRoleRepository
{
    private readonly FitTrackContext _context;

    public RoleRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<RoleEntity?> GetRoleByNameAsync(RoleEnum roleName)
    {
        return await _context.Roles
            .Where(r => r.RoleName == roleName)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
