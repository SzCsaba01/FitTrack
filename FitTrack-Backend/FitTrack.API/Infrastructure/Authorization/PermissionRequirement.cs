using Microsoft.AspNetCore.Authorization;

namespace FitTrack.API.Infrastructure.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyList<string> Permissions { get; }
    public PermissionRequirement(IEnumerable<string> permissions)
    {
        Permissions = permissions
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();
    }
}
