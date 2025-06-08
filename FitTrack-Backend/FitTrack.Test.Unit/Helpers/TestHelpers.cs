using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;

namespace FitTrack.Test.Unit.Helpers;

public static class TestHelpers
{
    //Requests
    public static RegistrationRequest CreateRegistrationRequest(
            string? username = null,
            string? email = null,
            string? password = null,
            string? confirmPassword = null,
            UnitSystemEnum unitSystem = UnitSystemEnum.Metric,
            double weightKg = 70,
            double heightCm = 170)
    {
        return new RegistrationRequest
        {
            Username = username ?? "testuser",
            Email = email ?? "testuser@example.com",
            Password = password ?? "P@ssw0rd!",
            ConfirmPassword = confirmPassword ?? "P@ssw0rd!",
            UnitSystem = unitSystem,
            WeightKg = weightKg,
            HeightCm = heightCm
        };
    }

    public static LoginRequest CreateLoginRequest(string credential = "testuser", string password = "P@ssw0rd!")
    {
        return new LoginRequest
        {
            Credential = credential,
            Password = password
        };
    }


    public static ChangePasswordRequest CreateChangePasswordRequest(
        string? token = null,
        string? password = null,
        string? confirmPassword = null)
    {
        return new ChangePasswordRequest
        {
            ChangePasswordToken = token ?? Guid.NewGuid().ToString(),
            Password = password ?? "NewP@ssw0rd!",
            ConfirmPassword = confirmPassword ?? "NewP@ssw0rd!"
        };
    }

    public static object InvokePrivateMethod(object instance, string methodName, params object[] parameters)
    {
        var method = instance.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (method == null) throw new Exception($"Method '{methodName}' not found");
        return method.Invoke(instance, parameters);
    }


    //Entities
    public static RoleEntity CreateRole(
        RoleEnum roleName = RoleEnum.User,
        Guid? id = null)
    {
        return new RoleEntity
        {
            Id = id ?? Guid.NewGuid(),
            RoleName = roleName,
            Permissions = new List<RolePermissionMapping>(),
            Users = new List<UserEntity>()
        };
    }

    public static PermissionEntity CreatePermission(
        string? name = null,
        Guid? id = null)
    {
        return new PermissionEntity
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "DefaultPermission",
            Roles = new List<RolePermissionMapping>()
        };
    }

    public static RolePermissionMapping CreateRolePermissionMapping(
        RoleEntity? role = null,
        PermissionEntity? permission = null)
    {
        role ??= CreateRole();
        permission ??= CreatePermission();

        return new RolePermissionMapping
        {
            RoleId = role.Id,
            PermissionId = permission.Id,
            Role = role,
            Permission = permission
        };
    }

    public static UserEntity CreateUser(
        Guid? id = null,
        string? username = null,
        string? email = null,
        RoleEntity? role = null,
        byte[]? hashedPassword = null,
        byte[]? salt = null,
        bool isEmailConfirmed = false,
        byte[]? refreshToken = null)
    {
        role ??= CreateRole();

        return new UserEntity
        {
            Id = id ?? Guid.NewGuid(),
            Username = username ?? "testuser",
            HashedPassword = hashedPassword ?? new byte[] { 1, 2, 3, 4 },
            Salt = salt ?? new byte[] { 5, 6, 7, 8 },
            ChangePasswordToken = null,
            ChangePasswordTokenExpiration = null,
            Email = email ?? "testuser@example.com",
            RegistrationDate = DateTime.UtcNow,
            EmailVerificationToken = null,
            EmailVerificationExpiration = null,
            isEmailConfirmed = isEmailConfirmed,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = null,
            RoleId = role.Id,
            Role = role,
        };
    }

    public static UserProfileEntity CreateUserProfile(
        Guid? id = null,
        Guid? userId = null,
        string? firstName = null,
        string? lastName = null,
        GenderEnum gender = GenderEnum.Male,
        DateTime? dateOfBirth = null,
        double heightCm = 170,
        double weightKg = 70,
        UserEntity? user = null)
    {
        userId ??= user?.Id ?? Guid.NewGuid();

        return new UserProfileEntity
        {
            Id = id ?? Guid.NewGuid(),
            UserId = userId.Value,
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            Gender = gender,
            DateOfBirth = dateOfBirth ?? DateTime.UtcNow.AddYears(-30),
            HeightCm = heightCm,
            WeightKg = weightKg,
            User = user
        };
    }

    public static UserPreferenceEntity CreateUserPreference(
        Guid? id = null,
        Guid? userId = null,
        UnitSystemEnum unitSystem = UnitSystemEnum.Metric,
        AppThemeEnum appTheme = AppThemeEnum.Dark,
        UserEntity? user = null)
    {
        userId ??= user?.Id ?? Guid.NewGuid();

        return new UserPreferenceEntity
        {
            Id = id ?? Guid.NewGuid(),
            UserId = userId.Value,
            UnitSystem = unitSystem,
            AppTheme = appTheme,
            User = user
        };
    }
}
