using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Services;
using MediatR; 
using Template.Application.Services.Interfaces;
using Template.Application.Options.Auth;
namespace Template.Application.ApplicationModule;
using Microsoft.Extensions.Options;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencies).Assembly));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}