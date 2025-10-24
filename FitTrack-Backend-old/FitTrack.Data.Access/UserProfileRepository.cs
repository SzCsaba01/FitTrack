using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly FitTrackContext _context;

    public UserProfileRepository(FitTrackContext context)
    {
        _context = context;
    }

    // TEST:
    public async Task<UserProfileEntity?> GetUserProfileWithUserAsyncByUserId(Guid userId)
    {
        return await _context.UserProfiles
            .Where(up => up.UserId == userId)
            .Include(up => up.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    // TEST:
    public async Task<UserProfileEntity?> GetUserProfileByUserIdAsync(Guid userId)
    {
        return await _context.UserProfiles
            .Where(up => up.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task CreateUserProfileAsync(UserProfileEntity userProfile)
    {
        await _context.UserProfiles.AddAsync(userProfile);
    }

    public Task UpdateUserProfileAsync(UserProfileEntity userProfile)
    {
        _context.UserProfiles.Update(userProfile);
        return Task.CompletedTask;
    }
}
