using Microsoft.AspNetCore.Authorization;

namespace FitTrack.API.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) => Policy = permission;
}
