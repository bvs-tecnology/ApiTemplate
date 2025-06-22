using Grafana.OpenTelemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infra.Security;

public static class OpenTelemetryInjector
{
    public static IServiceCollection AddOpenTemeletryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiName = configuration["ApiName"]!;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(apiName);
        services.AddOpenTelemetry()
            .WithTracing((traceBuilder) =>
            {
                traceBuilder
                    .AddSource(apiName)
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddNpgsql()
                    .UseGrafana();
            })
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .SetResourceBuilder(resourceBuilder)
                    .UseGrafana();
            });
        
        return services;
    }

    public static ILoggingBuilder AddOpenTelemetryConfiguration(this ILoggingBuilder logging, IConfiguration configuration)
    {
        var apiName = configuration["ApiName"]!;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(apiName);
        logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeFormattedMessage = true;
            options.AttachLogsToActivityEvent();
            options.UseGrafana();
        });
        return logging;
    }
}