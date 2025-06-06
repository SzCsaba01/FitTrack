using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Http;

namespace FitTrack.Service.Business;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEncryptionService encryptionService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetUserByUsernameOrEmailAsync(request.Credential);

        if (user == null)
        {
            throw new ModelNotFoundException("Invalid credentials!");
        }

        if (!user.isEmailConfirmed)
        {
            throw new ValidationException("Email is not confirmed!");
        }

        var hashedPassword = _encryptionService.HashString(request.Password, user.Salt);

        if (!user.HashedPassword.SequenceEqual(hashedPassword))
        {
            throw new ModelNotFoundException("Invalid credentials");
        }

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(AppConstants.ACCESS_TOKEN_VALIDATION_TIME_MINUTES);
        var claims = BuildUserClaims(user);

        var accessToken = _jwtService.GenerateJwt(claims, accessTokenExpiration);

        var refreshTokenExpiration = DateTime.UtcNow.AddDays(AppConstants.REFRESH_TOKEN_VALIDATION_TIME_DAYS);
        var refreshToken = _encryptionService.GenerateSecureToken();
        var hashedRefreshToken = _encryptionService.HashString(refreshToken);

        user.RefreshToken = hashedRefreshToken;
        user.RefreshTokenExpiration = refreshTokenExpiration;
        user.Role = null;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        SetTokenCookies(accessToken, accessTokenExpiration, refreshToken, refreshTokenExpiration);
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);

        httpContext.Response.Cookies.Delete("AccessToken");
        httpContext.Response.Cookies.Delete("RefreshToken");

        if (String.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var hashedRefreshToken = _encryptionService.HashString(refreshToken);
        var user = await _userRepository.GetUserByRefreshTokenAsync(hashedRefreshToken);

        if (user == null)
        {
            return;
        }

        user.RefreshTokenExpiration = null;
        user.RefreshToken = null;
        user.Role = null;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RefreshTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            httpContext.Response.Cookies.Delete("AccessToken");
            httpContext.Response.Cookies.Delete("RefreshToken");

            return;
        }

        var hashedRefreshToken = _encryptionService.HashString(refreshToken);

        var user = await _userRepository.GetUserByRefreshTokenAsync(hashedRefreshToken);

        if (user == null || user.RefreshTokenExpiration > DateTime.UtcNow)
        {
            httpContext.Response.Cookies.Delete("AccessToken");
            httpContext.Response.Cookies.Delete("RefreshToken");

            return;
        }


        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(AppConstants.ACCESS_TOKEN_VALIDATION_TIME_MINUTES);
        var claims = BuildUserClaims(user);

        var accessToken = _jwtService.GenerateJwt(claims, accessTokenExpiration);

        var newRefreshToken = _encryptionService.GenerateSecureToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(AppConstants.REFRESH_TOKEN_VALIDATION_TIME_DAYS);

        var hashedNewRefreshToken = _encryptionService.HashString(newRefreshToken);

        user.RefreshTokenExpiration = refreshTokenExpiration;
        user.RefreshToken = hashedRefreshToken;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        SetTokenCookies(accessToken, accessTokenExpiration, newRefreshToken, refreshTokenExpiration);
    }

    private ClaimsIdentity BuildUserClaims(UserEntity user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        foreach (var permission in user.Role.Permissions)
        {
            claims.Add(new Claim("Permission", permission.Permission.Name));
        }

        return new ClaimsIdentity(claims);
    }

    private void SetTokenCookies(string accessToken, DateTime accessTokenExpiration, string refreshToken, DateTime refreshTokenExpiration)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        httpContext.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = accessTokenExpiration,
        });

        httpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = refreshTokenExpiration
        });
    }
}
