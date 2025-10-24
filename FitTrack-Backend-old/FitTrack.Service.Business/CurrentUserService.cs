using System.Security.Claims;
using FitTrack.Service.Business.Exceptions;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Http;

namespace FitTrack.Service.Business;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var id = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
        {
            throw new AuthenticationException();
        }

        return Guid.Parse(id);
    }
}
