using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
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
    public async Task<IActionResult> GetUserData()
    {
        var response = await _userService.GetUserDataAsync();

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
    public async Task<IActionResult> VerifyEmailVerificationToken([FromQuery] string token)
    {
        await _userService.VerifyEmailVerificationTokenAsync(token);

        var message = new SuccessMessageResponse("You have successfully verified your email");

        return Ok(message);
    }

    [AllowAnonymous]
    [HttpPut("verify-change-password-token")]
    public async Task<IActionResult> VerifyChangePassworToken([FromQuery] string token)
    {
        await _userService.VerifyChangePasswordTokenAsync(token);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPut("send-forgot-password-email")]
    public async Task<IActionResult> SendForgotPasswordEmail([FromQuery] string email)
    {
        await _userService.SendForgotPasswordEmailAsync(email);

        var message = new SuccessMessageResponse("We have sent you an email with which you can reset your password!");

        return Ok(message);
    }

    [AllowAnonymous]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _userService.ChangePasswordAsync(request);

        var message = new SuccessMessageResponse("You have successfully changed your password!");

        return Ok(message);
    }
}
