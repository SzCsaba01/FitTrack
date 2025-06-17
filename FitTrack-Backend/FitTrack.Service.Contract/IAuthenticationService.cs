using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;

namespace FitTrack.Service.Contract;

public interface IAuthenticationService
{
    public Task<AuthenticationResponse> GetUserDataAsync();
    public Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    public Task LogoutAsync();
    public Task RefreshTokenAsync();
}
