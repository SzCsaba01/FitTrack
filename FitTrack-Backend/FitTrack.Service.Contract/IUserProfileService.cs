using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Service.Contract;

public interface IUserProfileService
{
    public Task<UserProfileResponse> GetUserProfileByUserIdAsync(Guid userId, UnitSystemEnum unitSystem);
    public Task UpdateUserProfileByUserIdAsync(Guid userId, UnitSystemEnum unitSystem, UpdateUserProfileRequest request);
}
