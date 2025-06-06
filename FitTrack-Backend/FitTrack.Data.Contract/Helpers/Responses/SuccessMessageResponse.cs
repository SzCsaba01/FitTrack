namespace FitTrack.Data.Contract.Helpers.Responses;

public class SuccessMessageResponse
{
    public string Message { get; set; }

    public SuccessMessageResponse(string message)
    {
        Message = message;
    }
}
