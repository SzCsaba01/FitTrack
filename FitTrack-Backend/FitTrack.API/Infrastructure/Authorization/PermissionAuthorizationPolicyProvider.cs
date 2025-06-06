using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FitTrack.API.Infrastructure.Authorization;

public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Try to get an already-defined policy first (optional, fallback)
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
            return policy;

        // Dynamically create a permission-based policy
        var permissions = policyName.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var policyBuilder = new AuthorizationPolicyBuilder();
        policyBuilder.AddRequirements(new PermissionRequirement(permissions));

        return policyBuilder.Build();
    }
}
