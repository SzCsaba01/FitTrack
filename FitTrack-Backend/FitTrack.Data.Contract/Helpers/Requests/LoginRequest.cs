namespace FitTrack.Data.Contract.Helpers.Requests;

public class LoginRequest
{
    public required string Credential { get; set; }
    public required string Password { get; set; }
}
