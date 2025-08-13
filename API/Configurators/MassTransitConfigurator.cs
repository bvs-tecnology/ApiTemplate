using System.Diagnostics;
using MassTransit;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace API.Configurators;

public static class MassTransitConfigurator
{
    public static IServiceCollection AddLocalMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });
                cfg.UseInstrumentation();
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}