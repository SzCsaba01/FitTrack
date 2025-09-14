namespace FitTrack.Data.Contract.Helpers.Requests;

public class GetFilteredUsersRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}
