using System.Diagnostics.CodeAnalysis;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API.Configurators;
[ExcludeFromCodeCoverage]
public static class OpenTelemetryConfigurator
{
    public static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiName = configuration["ApiName"]!;
        var otelUri = configuration["OpenTelemetryUrl"]!;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(apiName);
        services.AddOpenTelemetry()
            .WithTracing(traceBuilder =>
            {
                traceBuilder
                    .AddSource(apiName)
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMassTransitInstrumentation()
                    .AddSource("MassTransit")
                    .AddNpgsql()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(otelUri); 
                        opt.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter(apiName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(otelUri);
                        opt.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
        
        return services;
    }

    public static ILoggingBuilder AddOpenTelemetryConfiguration(this ILoggingBuilder logging, IConfiguration configuration)
    {
        var apiName = configuration["ApiName"]!;
        var otelUri = configuration["OpenTelemetryUrl"]!;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(apiName);
        logging.AddOpenTelemetry(loggingBuilder =>
        {
            loggingBuilder.IncludeFormattedMessage = true;
            loggingBuilder.SetResourceBuilder(resourceBuilder)
                .AttachLogsToActivityEvent()
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(otelUri);
                    opt.Protocol = OtlpExportProtocol.Grpc;
                });
        });
        return logging;
    }
}