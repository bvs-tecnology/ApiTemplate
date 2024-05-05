using System.Diagnostics.CodeAnalysis;
using Infra.Utils.Configuration;

namespace API.HealthCheck
{
    [ExcludeFromCodeCoverage]
    public static class HealthCheckBuilder
    {
        public static IHealthChecksBuilder AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = Builders.BuildConnectionString(configuration);
            var builder = services.AddHealthChecks().AddSqlServer(conn, name: "database");
            return builder;
        }
    }
}
