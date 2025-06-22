using System.Security.Claims;
using System.Text.Json;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Infra.Security.Helpers;

public class KeycloakClaimsTransformer(IOptionsSnapshot<Keycloak> keycloak) : IClaimsTransformation
{
    private readonly Keycloak _keycloak = keycloak.Value;

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        var resourceAccess = principal.FindFirst("resource_access");
        
        if (resourceAccess == null)
        {
            return Task.FromResult(principal);
        }
        
        var parsed = JsonDocument.Parse(resourceAccess.Value);

        if (string.IsNullOrEmpty(_keycloak.ClientId) ||
            !parsed.RootElement.TryGetProperty(_keycloak.ClientId, out var template) ||
            !template.TryGetProperty("roles", out var roles))
        {
            return Task.FromResult(principal);
        }
        
        foreach (var roleValue in roles.EnumerateArray().Select(role => role.GetString()).OfType<string>())
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
        }

        return Task.FromResult(principal);
    }
}