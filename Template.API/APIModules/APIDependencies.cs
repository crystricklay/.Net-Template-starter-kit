using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Options.Database;

namespace Template.API.APIModules;

public static class APIDependencies
{
   
        public static IServiceCollection AddConfigurationOptionsBindings(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<PostgresOptions>(configuration.GetSection("ConnectionStrings"));
            return services;
        }
    }
