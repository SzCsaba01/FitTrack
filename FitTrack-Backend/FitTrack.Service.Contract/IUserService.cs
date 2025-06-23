using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;

namespace FitTrack.Service.Contract;

public interface IUserService
{
    public Task<AuthenticationResponse> GetUserDataAsync();
    public Task RegisterUserAsync(RegistrationRequest request);
    public Task VerifyEmailVerificationTokenAsync(string emailVerificationToken);
    public Task SendForgotPasswordEmailAsync(string email);
    public Task VerifyChangePasswordTokenAsync(string resetPasswordToken);
    public Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
}
