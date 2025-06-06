using System.Security.Claims;

namespace FitTrack.Service.Contract;

public interface IJwtService
{
    public string GenerateJwt(ClaimsIdentity claims, DateTime expirationDate);
}
