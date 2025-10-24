using FitTrack.Data.Object.Enums;

namespace FitTrack.Service.Contract;

public interface IUserPreferenceService
{
    public Task UpdateThemeByUserIdAsync(Guid userId, AppThemeEnum newTheme);
    public Task UpdateUnitSystemByUserIdAsync(Guid userId, UnitSystemEnum newUnitSystem);
}
