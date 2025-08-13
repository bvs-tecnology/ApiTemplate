using API.Configurators;
using API.Middlewares;
using HealthChecks.UI.Client;
using Infra.IoC;
using Infra.Security;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region Injections
builder.Services
    .AddOpenTelemetryConfiguration(builder.Configuration)
    .AddLocalServices(builder.Configuration)
    .AddLocalMassTransit(builder.Configuration)
    .AddLocalHttpClients(builder.Configuration)
    .AddLocalUnitOfWork(builder.Configuration)
    .AddLocalCache(builder.Configuration)
    .AddLocalHealthChecks(builder.Configuration)
    .AddKeycloakAuthentication(builder.Configuration)
    .AddLocalCors()
    .AddOptions();
builder.Logging
    .AddOpenTelemetryConfiguration(builder.Configuration);
#endregion

var app = builder.Build();

#region Middlewares
app.UseMiddleware<ControllerMiddleware>();
app.UseHttpsRedirection();
app.UseLocalCors(builder.Environment);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RedisCacheMiddleware>();
app.MapOpenApi();
app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapControllers();
#endregion

app.Run();

