using API.Configurators;
using API.Middlewares;
using HealthChecks.UI.Client;
using Infra.IoC;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Injections

builder.Services
    .AddLocalOpenApi(builder.Configuration)
    .AddOpenTelemetryConfiguration(builder.Configuration)
    .InjectDependencies(builder.Configuration)
    .AddLocalMassTransit(builder.Configuration)
    .AddLocalHealthChecks(builder.Configuration)
    .AddKeycloakAuthentication(builder.Configuration)
    .AddLocalCors()
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

