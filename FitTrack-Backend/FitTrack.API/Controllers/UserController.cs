using FitTrack.API.Infrastructure.Authorization;
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
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get-user-data")]
    public async Task<IActionResult> GetUserDataAsync()
    {
        var response = await _userService.GetUserDataAsync();

        return Ok(response);
    }

    [HasPermission("user:manage")]
    [HttpPost("get-filtered-users")]
    public async Task<IActionResult> GetFilteredUsersAsync([FromBody] GetFilteredUsersRequest request)
    {
        var response = await _userService.GetFilteredUsersAsync(request);

        return Ok(response);
    }

    [HasPermission("user:manage")]
    [HttpGet("get-user-details")]
    public async Task<IActionResult> GetUserDetailsAsync([FromQuery] Guid userId, [FromQuery] UnitSystemEnum unitSystem)
    {
        var response = await _userService.GetUserDetailsByIdAsync(userId, unitSystem);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequest request)
    {
        await _userService.RegisterUserAsync(request);

        var message = new SuccessMessageResponse("You have successfully registered! Verify your email to be able to log in!");

        return Ok(message);
    }

    [AllowAnonymous]
    [HttpPut("verify-email-verification-token")]
    public async Task<IActionResult> VerifyEmailVerificationTokenAsync([FromQuery] string token)
    {
        await _userService.VerifyEmailVerificationTokenAsync(token);

        var message = new SuccessMessageResponse("You have successfully verified your email!");

        return Ok(message);
    }

    [AllowAnonymous]
    [HttpPut("verify-change-password-token")]
    public async Task<IActionResult> VerifyChangePassworTokenAsync([FromQuery] string token)
    {
        await _userService.VerifyChangePasswordTokenAsync(token);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPut("send-forgot-password-email")]
    public async Task<IActionResult> SendForgotPasswordEmailAsync([FromQuery] string email)
    {
        await _userService.SendForgotPasswordEmailAsync(email);

        var message = new SuccessMessageResponse("We have sent you an email with which you can reset your password!");

        return Ok(message);
    }

    [AllowAnonymous]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        await _userService.ChangePasswordAsync(request);

        var message = new SuccessMessageResponse("You have successfully changed your password!");

        return Ok(message);
    }
}
