using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IUserPreferenceRepository
{
    public Task<UserPreferenceEntity?> GetUserPreferenceByUserIdAsync(Guid userId);
    public Task CreateUserPreferenceAsync(UserPreferenceEntity userPreference);
    public Task UpdateUserPreferenceAsync(UserPreferenceEntity userPreference);
}
