using FitTrack.API.Infrastructure.Authorization;
using FitTrack.Data.Access;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Service.Business;
using FitTrack.Service.Business.Validators;
using FitTrack.Service.Contract;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace FitTrack.API.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IMuscleRepository, MuscleRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IExerciseImageRepository, ExerciseImageRepository>();
        services.AddScoped<IExerciseMuscleMappingRepository, ExerciseMuscleMappingRepository>();
        services.AddScoped<IInstructionRepository, InstructionRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitNormalizerService, UnitNormalizerService>();

        services.AddAutoMapper(typeof(Mapper));

        services.AddHttpContextAccessor();

        services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();

        services.AddScoped<ValidationActionFilter>();

        services.AddCors(options => options.AddPolicy(
            name: "Origins",
            policy =>
            {
                // policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
