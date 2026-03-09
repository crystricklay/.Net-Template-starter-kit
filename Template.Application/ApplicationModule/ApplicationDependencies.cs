using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Services;
using MediatR; 
using Template.Application.Services.Interfaces;

namespace Template.Application.ApplicationModule;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencies).Assembly));
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}