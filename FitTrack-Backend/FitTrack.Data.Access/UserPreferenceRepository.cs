using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;

public class UserPreferenceRepository : IUserPreferenceRepository
{
    private readonly FitTrackContext _context;

    public UserPreferenceRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<UserPreferenceEntity?> GetUserPreferenceByUserIdAsync(Guid userId)
    {
        return await _context.UserPreferences
            .Where(up => up.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task CreateUserPreferenceAsync(UserPreferenceEntity userPreference)
    {
        await _context.UserPreferences.AddAsync(userPreference);
    }

    public Task UpdateUserPreferenceAsync(UserPreferenceEntity userPreference)
    {
        _context.UserPreferences.Update(userPreference);
        return Task.CompletedTask;
    }
}
