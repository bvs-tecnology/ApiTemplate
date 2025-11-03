using API.Configurators;
using API.Middlewares;
using HealthChecks.UI.Client;
using Infra.IoC;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#region Configs
builder.Services.Configure<PushNotificationConfigs>(builder.Configuration.GetSection("PushNotification"));
#endregion

#region Injections
builder.Services.AddControllers();
builder.Services
    .AddOpenTelemetryConfiguration(builder.Configuration)
    .AddOpenApiConfiguration(builder.Configuration)
    .InjectDependencies(builder.Configuration)
    .AddMassTransitConfiguration(builder.Configuration)
    .AddHealthChecksConfiguration(builder.Configuration)
    .AddKeycloakConfiguration(builder.Configuration)
    .AddCorsConfiguration()
    .AddOptions();
builder.Logging
    .AddOpenTelemetryConfiguration(builder.Configuration);
#endregion

var app = builder.Build();

#region Middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<TraceMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ActivityStatusMiddleware>();
app.UseHttpsRedirection();
app.UseLocalCors(builder.Environment);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RedisCacheMiddleware>();
app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapControllers();
#endregion

await app.RunAsync();

