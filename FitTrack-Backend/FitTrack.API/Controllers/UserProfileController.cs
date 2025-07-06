using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly ICurrentUserService _currentUserService;

    public UserProfileController(IUserProfileService userProfileService, ICurrentUserService currentUserService)
    {
        _userProfileService = userProfileService;
        _currentUserService = currentUserService;
    }

    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetUserProfile([FromQuery] UnitSystemEnum unitSystem)
    {
        var userId = _currentUserService.GetCurrentUserId();

        var response = await _userProfileService.GetUserProfileByUserIdAsync(userId, unitSystem);

        return Ok(response);
    }
    [HttpPut("update-user-profile")]
    public async Task<IActionResult> UpdateUserProfile([FromQuery] UnitSystemEnum unitSystem, [FromBody] UpdateUserProfileRequest request)
    {
        var userId = _currentUserService.GetCurrentUserId();

        await _userProfileService.UpdateUserProfileByUserIdAsync(userId, unitSystem, request);
        var message = new SuccessMessageResponse("Successfully updated your profile!");
        return Ok(message);
    }
}
