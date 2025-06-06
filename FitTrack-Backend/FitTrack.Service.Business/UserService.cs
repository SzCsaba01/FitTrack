using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Configuration;

namespace FitTrack.Service.Business;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUserPreferenceRepository _userPreferenceRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IUnitNormalizerService _unitNormalizerService;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    private readonly string _frontendUrl;

    public UserService(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository,
        IUserPreferenceRepository userPreferenceRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IUnitNormalizerService unitNormalizerService,
        IEncryptionService encryptionService,
        IMapper mapper,
        IJwtService jwtService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
        _emailService = emailService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _unitNormalizerService = unitNormalizerService;
        _userPreferenceRepository = userPreferenceRepository;
        _encryptionService = encryptionService;

        var frontendUrl = configuration["App:FrontendBaseUrl"];

        if (String.IsNullOrEmpty(frontendUrl))
        {
            throw new ConfigurationException();
        }

        _frontendUrl = frontendUrl;
        _jwtService = jwtService;
        _roleRepository = roleRepository;
    }

    public async Task RegisterUserAsync(RegistrationRequest request)
    {
        var existingUsername = await _userRepository.GetUserByUsernameAsync(request.Username).ConfigureAwait(false);

        if (existingUsername != null)
        {
            throw new ValidationException("Username already exists!");
        }

        var existingEmail = await _userRepository.GetUserByEmailAsync(request.Email).ConfigureAwait(false);

        if (existingEmail != null)
        {
            throw new ValidationException("Email already exists!");
        }

        if (request.Password != request.ConfirmPassword)
        {
            throw new ValidationException("Passwords doesn't match!");
        }

        if (request.UnitSystem == UnitSystemEnum.Imperial)
        {
            request.WeightKg = _unitNormalizerService.ConvertToKg(request.WeightKg);
            request.HeightCm = _unitNormalizerService.ConvertToCm(request.HeightCm);
        }

        var userRole = await _roleRepository.GetRoleByNameAsync(RoleEnum.User);

        if (userRole == null)
        {
            throw new ConfigurationException();
        }

        var userEntity = _mapper.Map<UserEntity>(request);
        var userProfile = _mapper.Map<UserProfileEntity>(request);
        var userPreference = new UserPreferenceEntity()
        {
            AppTheme = AppThemeEnum.Light,
            UnitSystem = request.UnitSystem
        };

        userEntity.RoleId = userRole.Id;

        var salt = _encryptionService.GenerateSalt();
        var hashedPassword = _encryptionService.HashString(request.Password, salt);

        userEntity.HashedPassword = hashedPassword;
        userEntity.Salt = salt;

        var expirationDate = DateTime.UtcNow.AddHours(AppConstants.REGISTRATION_TOKEN_VALIDATION_TIME_HOURS);

        var emailVerificationToken = _encryptionService.GenerateSecureToken();
        var hashedToken = _encryptionService.HashString(emailVerificationToken);

        userEntity.RegistrationDate = DateTime.UtcNow;
        userEntity.EmailVerificationToken = hashedToken;
        userEntity.EmailVerificationExpiration = expirationDate;

        var url = Path.Combine(_frontendUrl, AppConstants.EMAIL_VERIFICATION_SUBJECT, emailVerificationToken);

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _userRepository.CreateUserAsync(userEntity);

            userProfile.UserId = userEntity.Id;
            userPreference.UserId = userEntity.Id;

            await _userProfileRepository.CreateUserProfileAsync(userProfile);
            await _userPreferenceRepository.CreateUserPreferenceAsync(userPreference);

            await _unitOfWork.SaveChangesAsync();

            var emailBody = _emailService.CreateRegistrationEmailBody(url, userEntity.Username);
            await _emailService.SendEmailAsync(AppConstants.VERIFY_EMAIL_URL, userEntity.Email, emailBody);

            await _unitOfWork.CommitTransactionAsync(transaction);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task VerifyEmailVerificationTokenAsync(string emailVerificationToken)
    {
        var hashedToken = _encryptionService.HashString(emailVerificationToken);

        var user = await _userRepository.GetUserByEmailVerificationTokenAsync(hashedToken);

        if (user == null ||
                (user.ChangePasswordTokenExpiration != null && user.ChangePasswordTokenExpiration < DateTime.UtcNow))
        {
            throw new ModelNotFoundException("Url is invalid or expired!");
        }

        user.EmailVerificationToken = null;
        user.EmailVerificationExpiration = null;
        user.isEmailConfirmed = true;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SendForgotPasswordEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            return;
        }

        var changePasswordTokenExpiration = DateTime.UtcNow.AddMinutes(AppConstants.CHANGE_PASSWORD_TOKEN_VALIDATION_TIME_MINUTES);
        var changePasswordToken = _encryptionService.GenerateSecureToken();
        var hashedToken = _encryptionService.HashString(changePasswordToken);

        user.ChangePasswordToken = hashedToken;
        user.ChangePasswordTokenExpiration = changePasswordTokenExpiration;

        var url = Path.Combine(_frontendUrl, AppConstants.CHANGE_PASSWORD_URL, changePasswordToken);

        var emailBody = _emailService.CreateForgotPasswordEmailBody(url, user.Username);

        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userRepository.UpdateUserAsync(user);

            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailAsync(AppConstants.CHANGE_PASSWORD_SUBJECT, user.Email, emailBody);

            await _unitOfWork.CommitTransactionAsync(transaction);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(transaction);
            throw;
        }
    }

    public async Task VerifyChangePasswordTokenAsync(string changePasswordToken)
    {
        var hashedToken = _encryptionService.HashString(changePasswordToken);
        var user = await _userRepository.GetUserByChangePasswordTokenAsync(hashedToken);

        if (user == null ||
                (user.ChangePasswordTokenExpiration != null && user.ChangePasswordTokenExpiration < DateTime.UtcNow))
        {
            throw new ModelNotFoundException("Change password url is invalid or expired!");
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
    {
        if (changePasswordRequest.Password != changePasswordRequest.ConfirmPassword)
        {
            throw new ValidationException("Passwords doesn't match!");
        }

        var hashedToken = _encryptionService.HashString(changePasswordRequest.ChangePasswordToken);

        var user = await _userRepository.GetUserByChangePasswordTokenAsync(hashedToken);

        if (user == null ||
                (user.ChangePasswordTokenExpiration != null && user.ChangePasswordTokenExpiration < DateTime.UtcNow))
        {
            throw new ModelNotFoundException("Change password url is invalid or expired!");
        }

        var newSalt = _encryptionService.GenerateSalt();
        var hashedPassword = _encryptionService.HashString(changePasswordRequest.Password, newSalt);

        user.Salt = newSalt;
        user.HashedPassword = hashedPassword;
        user.ChangePasswordTokenExpiration = null;
        user.ChangePasswordToken = null;

        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
