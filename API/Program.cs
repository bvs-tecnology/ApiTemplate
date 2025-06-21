using System.Text.Json.Serialization;
using API.Middlewares;
using API.Tracing;
using Grafana.OpenTelemetry;
using HealthChecks.UI.Client;
using Infra.IoC;
using Infra.Security;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: OpenTelemetryExtensions.ServiceName,
        serviceVersion: OpenTelemetryExtensions.ServiceVersion);
builder.Services.AddOpenTelemetry()
    .WithTracing((traceBuilder) =>
    {
        traceBuilder
            .AddSource(OpenTelemetryExtensions.ServiceName)
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddNpgsql()
            .AddConsoleExporter()
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

builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;
    options.AttachLogsToActivityEvent();
    options.AddConsoleExporter();
    options.UseGrafana();
});

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.Configure<Keycloak>(builder.Configuration.GetSection("Keycloak"));
builder.Services.AddOpenApi();

#region Local Injections
builder.Services.AddLocalServices(builder.Configuration);
builder.Services.AddLocalHttpClients(builder.Configuration);
builder.Services.AddLocalUnitOfWork(builder.Configuration);
builder.Services.AddLocalCache(builder.Configuration);
builder.Services.AddLocalHealthChecks(builder.Configuration);
#endregion

#region Security Injections
builder.Services.AddLocalSecurity(builder.Configuration);
builder.Services.AddLocalCors();
#endregion

builder.Services.AddOptions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowSpecificOrigin");
}

app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#region Middlewares
app.UseMiddleware<ControllerMiddleware>();
app.UseMiddleware<RedisCacheMiddleware>();
#endregion

app.Run();

