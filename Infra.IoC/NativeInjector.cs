using System.Diagnostics.CodeAnalysis;
using Domain.SeedWork.Notification;
using Infra.Data;
using Infra.Security.Helpers;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC
{
    [ExcludeFromCodeCoverage]
    public static class NativeInjector
    {
        public static IServiceCollection AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            
            services.AddScoped<INotification, Notification>();
            
            #endregion

            #region Repositories
            #endregion
            
            #region Transforms

            services.AddScoped<IClaimsTransformation, KeycloakClaimsTransformer>();

            #endregion
            
            return services;
        }

        public static IServiceCollection AddLocalUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseNpgsql(Builders.BuildPostgresConnectionString(configuration)));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddLocalCache(this IServiceCollection services, IConfiguration configuration) {
            services.AddStackExchangeRedisCache(options => options.Configuration = Builders.BuildRedisConnectionString(configuration));
            return services;
        }

        public static IServiceCollection AddLocalHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(Builders.BuildPostgresConnectionString(configuration))
                .AddRedis(Builders.BuildRedisConnectionString(configuration));
            return services;
        }
    }
}
