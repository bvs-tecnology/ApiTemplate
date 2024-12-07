using System.Diagnostics.CodeAnalysis;
using Application;
using Domain.SeedWork.Notification;
using Infra.Data;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infra.IoC
{
    [ExcludeFromCodeCoverage]
    public static class NativeInjector
    {
        public static void AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration) {}

        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            services.AddScoped<INotification, Notification>();
            services.AddSingleton<IContainer, ServiceProviderProxy>();
            #endregion

            #region Repositories
            #endregion
        }

        public static void AddLocalUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = Builders.BuildConnectionString(configuration);
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseSqlServer(connString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddLocalCache(this IServiceCollection services, IConfiguration configuration) {
            services.AddStackExchangeRedisCache(options => 
                options.Configuration = configuration.GetConnectionString("Redis")
            );
        }

        public static void AddLocalHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(Builders.BuildConnectionString(configuration))
                .AddRedis(configuration.GetConnectionString("Redis")!);
        }
    }
}
