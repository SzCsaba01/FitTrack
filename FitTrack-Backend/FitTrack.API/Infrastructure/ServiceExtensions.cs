using FitTrack.Data.Contract.Helpers;

namespace FitTrack.API.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Mapper));

        services.AddCors(options => options.AddPolicy(
            name: "Origins",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        return services;
    }
}
