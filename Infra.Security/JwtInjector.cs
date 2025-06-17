using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Security;

public static class JwtInjector
{
    public static void AddLocalSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["Keycloak:Issuer"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["Keycloak:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Keycloak:Issuer"],
                    NameClaimType = "preferred_username",
                    RoleClaimType = ClaimTypes.Role
                };
            });
        services.AddAuthorization();
    }
}