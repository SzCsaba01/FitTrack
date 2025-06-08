using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Business;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using FitTrack.Test.Unit.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrack.Test.Unit.ServiceTests;

public class UserServiceTests
{
    private readonly string _frontendBaseUrl = "http://test";

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserProfileRepository> _UserProfileRepositoryMock;
    private readonly Mock<IUserPreferenceRepository> _userPreferenceRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IUnitNormalizerService> _unitNormalizerServiceMock;
    private readonly Mock<IEncryptionService> _encryptionSeriveMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _UserProfileRepositoryMock = new Mock<IUserProfileRepository>();
        _userPreferenceRepositoryMock = new Mock<IUserPreferenceRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _emailServiceMock = new Mock<IEmailService>();
        _unitNormalizerServiceMock = new Mock<IUnitNormalizerService>();
        _encryptionSeriveMock = new Mock<IEncryptionService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();

        var config = new Dictionary<string, string?>
        {
            {"App:FrontendBaseUrl", _frontendBaseUrl}
        };

        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _UserProfileRepositoryMock.Object,
            _userPreferenceRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _emailServiceMock.Object,
            _unitNormalizerServiceMock.Object,
            _encryptionSeriveMock.Object,
            _mapperMock.Object,
            _jwtServiceMock.Object,
            configBuilder,
            _loggerMock.Object);
    }

    [Fact(DisplayName = "RegisterUserAsync with existing username throws ValidationException")]
    public async Task RegisterUserAsync_UsernameExists_ThrowsValidationException()
    {
        // Arrange
        var request = TestHelpers.CreateRegistrationRequest();
        var userEntity = TestHelpers.CreateUser();
        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(request.Username)).ReturnsAsync(userEntity);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _userService.RegisterUserAsync(request));
    }

    [Fact(DisplayName = "RegisterUserAsync with existing email throws ValidationException")]
    public async Task RegisterUserAsync_EmailExists_ThrowsValidationException()
    {
        // Arrange
        var request = TestHelpers.CreateRegistrationRequest();
        var userEntity = TestHelpers.CreateUser();
        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(request.Username)).ReturnsAsync((UserEntity)null);
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(request.Email)).ReturnsAsync(userEntity);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _userService.RegisterUserAsync(request));
    }

    [Fact(DisplayName = "RegisterUserAsync with mismatched passwords throws ValidationException")]
    public async Task RegisterUserAsync_Throws_When_Passwords_Do_Not_Match()
    {
        // Arrange
        var request = TestHelpers.CreateRegistrationRequest(password: "pass1", confirmPassword: "pass2");
        _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(request.Username)).ReturnsAsync((UserEntity)null);
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(request.Email)).ReturnsAsync((UserEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _userService.RegisterUserAsync(request));
    }

    [Fact(DisplayName = "RegisterUserAsync with valid request creates user and sends email")]
    public async Task RegisterUserAsync_ValidRequest_CreatesUserAndSendsEmail()
    {
        // Arrange
        var request = TestHelpers.CreateRegistrationRequest();
        var userRole = TestHelpers.CreateRole(RoleEnum.User);
        var userEntity = TestHelpers.CreateUser();
        var userProfile = TestHelpers.CreateUserProfile();

        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(request.Username)).ReturnsAsync((UserEntity?)null);
        _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((UserEntity?)null);
        _roleRepositoryMock.Setup(x => x.GetRoleByNameAsync(userRole.RoleName)).ReturnsAsync(userRole);
        _mapperMock.Setup(x => x.Map<UserEntity>(request)).Returns(userEntity);
        _mapperMock.Setup(x => x.Map<UserProfileEntity>(request)).Returns(userProfile);
        _encryptionSeriveMock.Setup(x => x.GenerateSalt(16)).Returns(new byte[] { 1 });
        _encryptionSeriveMock.Setup(x => x.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(new byte[] { 2 });
        _encryptionSeriveMock.Setup(x => x.GenerateSecureToken(32)).Returns("securetoken");
        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());


        // Act
        await _userService.RegisterUserAsync(request);

        // Assert
        _userRepositoryMock.Verify(x => x.CreateUserAsync(It.IsAny<UserEntity>()), Times.Once);
        _UserProfileRepositoryMock.Verify(x => x.CreateUserProfileAsync(It.IsAny<UserProfileEntity>()), Times.Once);
        _userPreferenceRepositoryMock.Verify(x => x.CreateUserPreferenceAsync(It.IsAny<UserPreferenceEntity>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact(DisplayName = "VerifyEmailVerificationTokenAsync with invalid token throws ModelNotFoundException")]
    public async Task VerifyEmailVerificationTokenAsync_InvalidToken_ThrowsModelNotFoundException()
    {
        // Arrange
        var hash = new byte[] { 2 };
        _encryptionSeriveMock.Setup(x => x.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(hash);
        _userRepositoryMock.Setup(x => x.GetUserByEmailVerificationTokenAsync(hash)).ReturnsAsync((UserEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ModelNotFoundException>(() => _userService.VerifyEmailVerificationTokenAsync("token"));
    }

    [Fact(DisplayName = "VerifyEmailVerificationTokenAsync with valid token confirms email")]
    public async Task VerifyEmailVerificationTokenAsync_ValidToken_ConfirmsEmail()
    {
        // Arrange
        var token = "token";
        var hashedToken = new byte[] { 1 };
        var user = TestHelpers.CreateUser();

        _encryptionSeriveMock.Setup(x => x.HashString(token, It.IsAny<byte[]>())).Returns(hashedToken);
        _userRepositoryMock.Setup(x => x.GetUserByEmailVerificationTokenAsync(hashedToken)).ReturnsAsync(user);

        // Act
        await _userService.VerifyEmailVerificationTokenAsync(token);

        // Assert
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.Is<UserEntity>(u => u.isEmailConfirmed)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact(DisplayName = "SendForgotPasswordEmailAsync with unknown email skips email send")]
    public async Task SendForgotPasswordEmailAsync_EmailNotFound_SkipsEmailSend()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserEntity?)null);

        // Act
        await _userService.SendForgotPasswordEmailAsync("unknown@example.com");

        // Assert
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact(DisplayName = "SendForgotPasswordEmailAsync with valid email sends reset email")]
    public async Task SendForgotPasswordEmailAsync_ValidEmail_SendsResetEmail()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        var hash = new byte[] { 1 };
        _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);
        _encryptionSeriveMock.Setup(x => x.GenerateSecureToken(32)).Returns("securetoken");
        _encryptionSeriveMock.Setup(x => x.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(hash);
        _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Mock.Of<IDbContextTransaction>());

        // Act
        await _userService.SendForgotPasswordEmailAsync(user.Email);

        // Assert
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<UserEntity>()), Times.Once);
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
    }

    [Fact(DisplayName = "VerifyChangePasswordTokenAsync with expired token throws ModelNotFoundException")]
    public async Task VerifyChangePasswordTokenAsync_ExpiredToken_ThrowsModelNotFoundException()
    {
        // Arrange
        var expiredUser = TestHelpers.CreateUser();
        var hash = new byte[] { 1 };
        expiredUser.ChangePasswordTokenExpiration = DateTime.UtcNow.AddMinutes(-1);
        _encryptionSeriveMock.Setup(e => e.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(hash);
        _userRepositoryMock.Setup(r => r.GetUserByChangePasswordTokenAsync(hash)).ReturnsAsync(expiredUser);

        // Act & Assert
        await Assert.ThrowsAsync<ModelNotFoundException>(() => _userService.VerifyChangePasswordTokenAsync("expiredToken"));
    }

    [Fact(DisplayName = "VerifyChangePasswordTokenAsync with valid token returns successfully")]
    public async Task VerifyChangePasswordTokenAsync_ValidToken_ReturnsSuccessfully()
    {
        // Arrange
        var token = "token";
        var hash = new byte[] { 1 };
        var user = TestHelpers.CreateUser();

        _encryptionSeriveMock.Setup(x => x.HashString(token, It.IsAny<byte[]>())).Returns(hash);
        _userRepositoryMock.Setup(x => x.GetUserByChangePasswordTokenAsync(hash)).ReturnsAsync(user);

        // Act
        await _userService.VerifyChangePasswordTokenAsync(token);

        // Assert
        _userRepositoryMock.Verify(x => x.GetUserByChangePasswordTokenAsync(hash), Times.Once);
    }

    // ChangePasswordAsync
    [Fact(DisplayName = "ChangePasswordAsync with mismatched passwords throws ValidationException")]
    public async Task ChangePasswordAsync_PasswordMismatch_ThrowsValidationException()
    {
        // Arrange
        var req = TestHelpers.CreateChangePasswordRequest("token", "pass1", "pass2");

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _userService.ChangePasswordAsync(req));
    }

    [Fact(DisplayName = "ChangePasswordAsync with valid request updates user password")]
    public async Task ChangePasswordAsync_ValidRequest_UpdatesUserPassword()
    {
        // Arrange
        var request = TestHelpers.CreateChangePasswordRequest();
        var user = TestHelpers.CreateUser();
        var hash = new byte[] { 1 };
        var newHash = new byte[] { 2 };

        _encryptionSeriveMock.Setup(x => x.HashString(request.ChangePasswordToken, It.IsAny<byte[]>())).Returns(hash);
        _userRepositoryMock.Setup(x => x.GetUserByChangePasswordTokenAsync(hash)).ReturnsAsync(user);
        _encryptionSeriveMock.Setup(x => x.GenerateSalt(16)).Returns(new byte[] { 2 });
        _encryptionSeriveMock.Setup(x => x.HashString(request.Password, It.IsAny<byte[]>())).Returns(newHash);

        // Act
        await _userService.ChangePasswordAsync(request);

        // Assert
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.Is<UserEntity>(u => u.HashedPassword == newHash)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
