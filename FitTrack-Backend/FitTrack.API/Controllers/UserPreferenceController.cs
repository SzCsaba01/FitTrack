using FitTrack.Data.Object.Enums;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserPreferenceController : ControllerBase
{
    private readonly IUserPreferenceService _userPreferenceService;
    private readonly ICurrentUserService _currentUserService;

    public UserPreferenceController(IUserPreferenceService userPreferenceService, ICurrentUserService currentUserService)
    {
        _userPreferenceService = userPreferenceService;
        _currentUserService = currentUserService;
    }

    [HttpPut("update-app-theme")]
    public async Task<IActionResult> UpdateAppTheme([FromQuery] AppThemeEnum newTheme)
    {
        var userId = _currentUserService.GetCurrentUserId();

        await _userPreferenceService.UpdateThemeByUserIdAsync(userId, newTheme);

        return Ok();
    }

    [HttpPut("update-unit-system")]
    public async Task<IActionResult> UpdateUnitSystem([FromQuery] UnitSystemEnum newUnitSystem)
    {
        var userId = _currentUserService.GetCurrentUserId();

        await _userPreferenceService.UpdateUnitSystemByUserIdAsync(userId, newUnitSystem);

        return Ok();
    }

}
