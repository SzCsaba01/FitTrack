using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPut("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authenticationService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPut("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authenticationService.LogoutAsync();

        return Ok();
    }

    [HttpPut("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        await _authenticationService.RefreshTokenAsync();

        return Ok();
    }
}
