using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infra.Security;

public static class SwaggerInjector
{
    public static void AddLocalSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("0.0.1",
                new OpenApiInfo
                {
                    Title = "Template API",
                    Version = "0.0.1",
                    Description = "API responsavel pela autenticação do dominio Air Finder",
                    Contact = new OpenApiContact { Name = "Air Finder" }
                });
        });
    }
}