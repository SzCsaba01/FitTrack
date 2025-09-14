namespace FitTrack.Data.Contract.Helpers.Requests;

public class ChangePasswordRequest
{
    public required string ChangePasswordToken { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}
