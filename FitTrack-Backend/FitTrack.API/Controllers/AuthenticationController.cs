using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[Route("ap/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPut("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        await _authenticationService.LoginAsync(request);

        return Ok();
    }

    [HttpPut("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _authenticationService.LogoutAsync();

        return Ok();
    }

    [HttpPut("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        await _authenticationService.RefreshTokenAsync();

        return Ok();
    }
}
