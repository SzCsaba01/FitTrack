namespace FitTrack.Data.Contract.Helpers.Requests;

public class ChangePasswordRequest
{
    public string ChangePasswordToken { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
