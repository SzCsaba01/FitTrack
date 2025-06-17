using System.Security.Claims;
using AutoMapper;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using FitTrack.Service.Business;
using FitTrack.Service.Contract;
using FitTrack.Test.Unit.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrack.Test.Unit.ServiceTests;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<ILogger<AuthenticationService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<AuthenticationService>>();
        _mapperMock = new Mock<IMapper>();

        var context = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

        _authenticationService = new AuthenticationService(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _encryptionServiceMock.Object,
            _jwtServiceMock.Object,
            _httpContextAccessorMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact(DisplayName = "LoginAsync throws exception if user not found")]
    public async Task LoginAsync_UserNotFound_ThrowsValidationException()
    {
        // Arrange
        var request = TestHelpers.CreateLoginRequest();
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameOrEmailAsync(request.Credential)).ReturnsAsync((UserEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _authenticationService.LoginAsync(request));
    }

    [Fact(DisplayName = "LoginAsync throws if email not confirmed")]
    public async Task LoginAsync_EmailNotConfirmed_ThrowsValidationException()
    {
        // Arrange
        var request = TestHelpers.CreateLoginRequest();
        var user = TestHelpers.CreateUser();
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameOrEmailAsync(request.Credential)).ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _authenticationService.LoginAsync(request));
    }

    [Fact(DisplayName = "LoginAsync throws if password is incorrect")]
    public async Task LoginAsync_WrongPassword_ThrowsValidationException()
    {
        // Arrange
        var request = TestHelpers.CreateLoginRequest();
        var user = TestHelpers.CreateUser();
        user.isEmailConfirmed = true;
        user.Salt = new byte[] { 1, 2, 3 };
        user.HashedPassword = new byte[] { 4, 5, 6 };
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameOrEmailAsync(request.Credential)).ReturnsAsync(user);
        _encryptionServiceMock.Setup(enc => enc.HashString(request.Password, user.Salt)).Returns([9, 9, 9]);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _authenticationService.LoginAsync(request));
    }


    [Fact(DisplayName = "LogoutAsync clears tokens and updates user if refresh token exists")]
    public async Task LogoutAsync_ValidToken_LogsOutUser()
    {
        // Arrange
        var token = "refreshtoken";
        var hashedToken = new byte[] { 1, 2, 3 };
        var user = TestHelpers.CreateUser();

        // Mock Request.Cookies to contain the refresh token
        var requestCookiesMock = new Mock<IRequestCookieCollection>();
        string outVal = token;
        requestCookiesMock.Setup(c => c.TryGetValue("RefreshToken", out outVal)).Returns(true);

        // Mock Response.Cookies to verify delete calls
        var responseCookiesMock = new Mock<IResponseCookies>();
        responseCookiesMock.Setup(c => c.Delete(It.IsAny<string>()));

        var responseMock = new Mock<HttpResponse>();
        responseMock.SetupGet(r => r.Cookies).Returns(responseCookiesMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request.Cookies).Returns(requestCookiesMock.Object);
        httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        _encryptionServiceMock.Setup(e => e.HashString(token, It.IsAny<byte[]>())).Returns(hashedToken);
        _userRepositoryMock.Setup(r => r.GetUserByRefreshTokenAsync(hashedToken)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);

        // Act
        await _authenticationService.LogoutAsync();

        // Assert
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        responseCookiesMock.Verify(c => c.Delete("AccessToken"), Times.Once);
        responseCookiesMock.Verify(c => c.Delete("RefreshToken"), Times.Once);
    }



    [Fact(DisplayName = "RefreshTokenAsync sets new tokens if valid refresh token")]
    public async Task RefreshTokenAsync_ValidToken_RefreshesToken()
    {
        // Arrange
        var token = "refresh";
        var hashedToken = new byte[] { 1, 2, 3 };

        var user = TestHelpers.CreateUser();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(5);

        // Mock Request.Cookies to return the refresh token
        var requestCookiesMock = new Mock<IRequestCookieCollection>();
        string outVal = token;
        requestCookiesMock.Setup(c => c.TryGetValue("RefreshToken", out outVal)).Returns(true);

        // Mock Response.Cookies to verify append calls
        var responseCookiesMock = new Mock<IResponseCookies>();
        responseCookiesMock.Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));

        var responseMock = new Mock<HttpResponse>();
        responseMock.SetupGet(r => r.Cookies).Returns(responseCookiesMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request.Cookies).Returns(requestCookiesMock.Object);
        httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        // Setup encryption and repository mocks
        _encryptionServiceMock.Setup(e => e.HashString(token, It.IsAny<byte[]>())).Returns(hashedToken);
        _userRepositoryMock.Setup(r => r.GetUserByRefreshTokenAsync(hashedToken)).ReturnsAsync(user);

        _encryptionServiceMock.Setup(e => e.GenerateSecureToken(32)).Returns("newtoken");
        _encryptionServiceMock.Setup(e => e.HashString("newtoken", It.IsAny<byte[]>())).Returns(new byte[] { 9, 9, 9 });

        _jwtServiceMock.Setup(j => j.GenerateJwt(It.IsAny<ClaimsIdentity>(), It.IsAny<DateTime>())).Returns("jwt");

        _userRepositoryMock.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);

        // Act
        await _authenticationService.RefreshTokenAsync();

        // Assert
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        responseCookiesMock.Verify(c => c.Append("AccessToken", "jwt", It.IsAny<CookieOptions>()), Times.Once);
        responseCookiesMock.Verify(c => c.Append("RefreshToken", "newtoken", It.IsAny<CookieOptions>()), Times.Once);
    }


    [Fact(DisplayName = "BuildUserClaims returns claims with user ID and permissions")]
    public void BuildUserClaims_ReturnsClaimsWithUserIdAndPermissions()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        user.Role = new RoleEntity
        {
            PermissionMappings = new List<RolePermissionMapping>
        {
            new RolePermissionMapping { Permission = new PermissionEntity { Name = "Permission1" } },
            new RolePermissionMapping { Permission = new PermissionEntity { Name = "Permission2" } },
        }
        };

        // Act
        var result = (ClaimsIdentity)TestHelpers.InvokePrivateMethod(_authenticationService, "BuildUserClaims", user);

        // Assert
        var claims = result.Claims.ToList();

        Assert.Contains(claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        Assert.Contains(claims, c => c.Type == "Permission" && c.Value == "Permission1");
        Assert.Contains(claims, c => c.Type == "Permission" && c.Value == "Permission2");
    }

    [Fact(DisplayName = "SetTokenCookies sets AccessToken and RefreshToken cookies with correct options")]
    public void SetTokenCookies_SetsAccessAndRefreshTokenCookiesWithExpectedOptions()
    {
        // Arrange
        var responseCookiesMock = new Mock<IResponseCookies>();

        var responseMock = new Mock<HttpResponse>();
        responseMock.SetupGet(r => r.Cookies).Returns(responseCookiesMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        string accessToken = "access-token";
        DateTime accessTokenExp = DateTime.UtcNow.AddMinutes(5);
        string refreshToken = "refresh-token";
        DateTime refreshTokenExp = DateTime.UtcNow.AddDays(7);

        // Act
        TestHelpers.InvokePrivateMethod(_authenticationService, "SetTokenCookies",
            accessToken, accessTokenExp, refreshToken, refreshTokenExp);

        // Assert
        responseCookiesMock.Verify(c => c.Append(
            "AccessToken",
            accessToken,
            It.Is<CookieOptions>(opts => opts.Expires == accessTokenExp && opts.HttpOnly && opts.SameSite == SameSiteMode.Strict)
        ), Times.Once);

        responseCookiesMock.Verify(c => c.Append(
            "RefreshToken",
            refreshToken,
            It.Is<CookieOptions>(opts => opts.Expires == refreshTokenExp && opts.HttpOnly && opts.SameSite == SameSiteMode.Strict)
        ), Times.Once);
    }
}
