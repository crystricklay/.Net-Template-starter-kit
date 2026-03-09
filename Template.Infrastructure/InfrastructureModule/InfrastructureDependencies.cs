using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Interfaces.Repositories;
using Template.Domain.Interfaces.UnitOfWork;
using Template.Infrastructure.Repositories;
using Npgsql;
using Template.Infrastructure.Persistence;

namespace Template.Infrastructure.InfrastructureModule;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("DefaultConnection string is not configured");
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}