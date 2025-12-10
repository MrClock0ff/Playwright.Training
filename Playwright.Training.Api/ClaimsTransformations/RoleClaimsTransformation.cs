using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace Playwright.Training.Api.ClaimsTransformations;

public class RoleClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity? identity = principal.Identity as ClaimsIdentity;
        Claim? realmAccessClaim = identity?.FindFirst("realm_access");

        if (realmAccessClaim != null)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            RealmAccess? realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim.Value, jsonSerializerOptions);

            if (realmAccess?.Roles != null)
            {
                foreach (string role in realmAccess.Roles)
                {
                    identity?.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }
        
        return Task.FromResult(principal);
    }
}

public class RealmAccess
{
    public List<string>? Roles { get; set; }
}