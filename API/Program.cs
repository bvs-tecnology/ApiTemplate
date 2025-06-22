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
    .AddOpenTemeletryConfiguration(builder.Configuration)
    .AddLocalServices(builder.Configuration)
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

app.MapOpenApi();
app.UseLocalCors(builder.Environment);
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

