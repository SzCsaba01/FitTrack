using FitTrack.API.Infrastructure.Authorization;
using FitTrack.Data.Access;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Service.Business;
using FitTrack.Service.Contract;
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

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitNormalizerService, UnitNormalizerService>();

        services.AddAutoMapper(typeof(Mapper));

        services.AddHttpContextAccessor();

        services.AddCors(options => options.AddPolicy(
            name: "Origins",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
