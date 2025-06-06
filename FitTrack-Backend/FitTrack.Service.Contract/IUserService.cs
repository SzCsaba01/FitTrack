using FitTrack.Data.Contract.Helpers.Requests;

namespace FitTrack.Service.Contract;

public interface IUserService
{
    public Task RegisterUserAsync(RegistrationRequest request);
    public Task VerifyEmailVerificationTokenAsync(string emailVerificationToken);
    public Task SendForgotPasswordEmailAsync(string email);
    public Task VerifyChangePasswordTokenAsync(string resetPasswordToken);
    public Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
}
