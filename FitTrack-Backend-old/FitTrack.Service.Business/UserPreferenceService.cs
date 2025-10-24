using FitTrack.Data.Contract;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;
public class UserPreferenceService : IUserPreferenceService
{
    private readonly IUserPreferenceRepository _userPreferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserPreferenceService> _logger;

    public UserPreferenceService(
        IUserPreferenceRepository userPreferenceRepository, IUnitOfWork unitOfWork,
        ILogger<UserPreferenceService> logger)
    {
        _userPreferenceRepository = userPreferenceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task UpdateThemeByUserIdAsync(Guid userId, AppThemeEnum newTheme)
    {
        var userPreference = await _userPreferenceRepository.GetUserPreferenceByUserIdAsync(userId);

        if (userPreference == null)
        {
            throw new ModelNotFoundException();
        }

        userPreference.AppTheme = newTheme;

        await _userPreferenceRepository.UpdateUserPreferenceAsync(userPreference);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUnitSystemByUserIdAsync(Guid userId, UnitSystemEnum newUnitSystem)
    {
        var userPreference = await _userPreferenceRepository.GetUserPreferenceByUserIdAsync(userId);

        if (userPreference == null)
        {
            throw new ModelNotFoundException();
        }

        userPreference.UnitSystem = newUnitSystem;

        await _userPreferenceRepository.UpdateUserPreferenceAsync(userPreference);
        await _unitOfWork.SaveChangesAsync();
    }
}
