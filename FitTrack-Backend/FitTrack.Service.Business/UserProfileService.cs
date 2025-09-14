using AutoMapper;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUnitNormalizerService _unitNormalizerService;
    private readonly ILogger<UserProfileService> _logger;
    private readonly IMapper _mapper;

    public UserProfileService(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork,
        IUnitNormalizerService unitNormalizerService,
        ILogger<UserProfileService> logger,
        IMapper mapper)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
        _unitNormalizerService = unitNormalizerService;
        _logger = logger;
        _mapper = mapper;
    }

    // TEST:
    public async Task<UserProfileResponse> GetUserProfileByUserIdAsync(Guid userId, UnitSystemEnum unitSystem)
    {
        var userProfile = await _userProfileRepository.GetUserProfileWithUserAsyncByUserId(userId).ConfigureAwait(false);

        if (userProfile == null)
        {
            throw new ModelNotFoundException();
        }

        var response = _mapper.Map<UserProfileResponse>(userProfile);

        if (unitSystem == UnitSystemEnum.Imperial)
        {
            response.Weight = _unitNormalizerService.ConvertKgToLb(response.Weight);
            response.Height = _unitNormalizerService.ConvertCmToInch(response.Height);
        }

        return response;
    }

    // TEST:
    public async Task UpdateUserProfileByUserIdAsync(Guid userId, UnitSystemEnum unitSystem, UpdateUserProfileRequest request)
    {
        var userProfile = await _userProfileRepository.GetUserProfileByUserIdAsync(userId).ConfigureAwait(false);

        if (userProfile == null)
        {
            throw new ModelNotFoundException();
        }

        double weight = request.Weight;
        double height = request.Height;

        if (unitSystem == UnitSystemEnum.Imperial)
        {
            weight = _unitNormalizerService.ConvertLbToKg(request.Weight);
            height = _unitNormalizerService.ConvertInchToCm(request.Height);
        }

        userProfile.HeightCm = height;
        userProfile.WeightKg = weight;
        userProfile.FirstName = request.FirstName;
        userProfile.LastName = request.LastName;
        userProfile.Gender = request.Gender;
        userProfile.DateOfBirth = request.DateOfBirth;

        await _userProfileRepository.UpdateUserProfileAsync(userProfile).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
