using System.Security.Claims;
using AutoMapper;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Data.Object.Entities;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEncryptionService encryptionService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenticationService> logger,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> GetUserDataAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new ConfigurationException();
        }

        httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);

        if (refreshToken == null)
        {
            throw new AuthenticationException();
        }

        var hashedToken = _encryptionService.HashString(refreshToken);
        var user = await _userRepository.GetUserByRefreshTokenAsync(hashedToken);

        if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            throw new AuthenticationException();
        }

        var response = _mapper.Map<AuthenticationResponse>(user);

        return response;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for: {Credential}", request.Credential);

        var user = await _userRepository.GetUserByUsernameOrEmailAsync(request.Credential);

        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found");
            throw new ValidationException("Invalid credentials!");
        }

        if (!user.isEmailConfirmed)
        {
            _logger.LogWarning("Login failed: Email not confirmed for user {UserId}", user.Id);
            throw new ValidationException("Email is not confirmed!");
        }

        var hashedPassword = _encryptionService.HashString(request.Password, user.Salt);

        if (!user.HashedPassword.SequenceEqual(hashedPassword))
        {
            _logger.LogWarning("Login failed: Invalid password for user {UserId}", user.Id);
            throw new ValidationException("Invalid credentials");
        }

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(AppConstants.ACCESS_TOKEN_VALIDATION_TIME_MINUTES);
        var claims = BuildUserClaims(user);

        var accessToken = _jwtService.GenerateJwt(claims, accessTokenExpiration);

        var refreshTokenExpiration = DateTime.UtcNow.AddDays(AppConstants.REFRESH_TOKEN_VALIDATION_TIME_DAYS);
        var refreshToken = _encryptionService.GenerateSecureToken();
        var hashedRefreshToken = _encryptionService.HashString(refreshToken);

        var response = _mapper.Map<AuthenticationResponse>(user);

        user.RefreshToken = hashedRefreshToken;
        user.RefreshTokenExpiration = refreshTokenExpiration;
        user.Role = null;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        SetTokenCookies(accessToken, accessTokenExpiration, refreshToken, refreshTokenExpiration);

        _logger.LogInformation("User {UserId} successfully logged in", user.Id);

        return response;
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new ConfigurationException();
        }

        httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);

        httpContext.Response.Cookies.Delete("AccessToken");
        httpContext.Response.Cookies.Delete("RefreshToken");

        if (String.IsNullOrEmpty(refreshToken))
        {
            _logger.LogInformation("Logout completed: no refresh token found.");
            return;
        }

        var hashedRefreshToken = _encryptionService.HashString(refreshToken);
        var user = await _userRepository.GetUserByRefreshTokenAsync(hashedRefreshToken);

        if (user == null)
        {
            _logger.LogInformation("Logout: no user found with provided refresh token.");
            return;
        }

        user.RefreshTokenExpiration = null;
        user.RefreshToken = null;
        user.Role = null;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} logged out successfully", user.Id);
    }

    public async Task RefreshTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new ConfigurationException();
        }

        if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            _logger.LogWarning("RefreshToken failed: Refresh token cookie missing");
            httpContext.Response.Cookies.Delete("AccessToken");
            httpContext.Response.Cookies.Delete("RefreshToken");
            return;
        }

        var hashedRefreshToken = _encryptionService.HashString(refreshToken);

        var user = await _userRepository.GetUserByRefreshTokenAsync(hashedRefreshToken);

        if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            _logger.LogWarning("RefreshToken failed: Invalid or expired token");
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
        user.RefreshToken = hashedNewRefreshToken;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        SetTokenCookies(accessToken, accessTokenExpiration, newRefreshToken, refreshTokenExpiration);

        _logger.LogInformation("User {UserId} successfully refreshed token", user.Id);
    }

    private ClaimsIdentity BuildUserClaims(UserEntity user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        foreach (var permission in user.Role.PermissionMappings)
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

        _logger.LogDebug("Token cookies set: AccessToken (expires at {AccessTokenExp}), RefreshToken (expires at {RefreshTokenExp})",
            accessTokenExpiration, refreshTokenExpiration);
    }
}
