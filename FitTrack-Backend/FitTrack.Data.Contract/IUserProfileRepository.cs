using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IUserProfileRepository
{
    public Task<UserProfileEntity?> GetUserProfileWithUserAsyncByUserId(Guid userId);
    public Task<UserProfileEntity?> GetUserProfileByUserIdAsync(Guid userId);
    public Task CreateUserProfileAsync(UserProfileEntity userProfile);
    public Task UpdateUserProfileAsync(UserProfileEntity userProfile);
}
