using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Access;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly FitTrackContext _context;

    public UserProfileRepository(FitTrackContext context)
    {
        _context = context;
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
