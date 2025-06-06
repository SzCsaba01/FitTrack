using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IUserProfileRepository
{
    public Task CreateUserProfileAsync(UserProfileEntity userProfile);
    public Task UpdateUserProfileAsync(UserProfileEntity userProfile);
}
