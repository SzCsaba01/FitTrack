using FitTrack.Data.Contract.Helpers.Requests;

namespace FitTrack.Service.Contract;

public interface IAuthenticationService
{
    public Task LoginAsync(LoginRequest request);
    public Task LogoutAsync();
    public Task RefreshTokenAsync();
}
