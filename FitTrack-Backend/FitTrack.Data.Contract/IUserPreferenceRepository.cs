using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IUserPreferenceRepository
{
    public Task CreateUserPreferenceAsync(UserPreferenceEntity userPreference);
    public Task UpdateUserPreferenceAsync(UserPreferenceEntity userPreference);
}
