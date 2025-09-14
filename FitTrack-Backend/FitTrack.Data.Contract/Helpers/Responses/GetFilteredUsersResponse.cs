using FitTrack.Data.Contract.Helpers.DTOs;

namespace FitTrack.Data.Contract.Helpers.Responses;

public class GetFilteredUsersResponse
{
    public List<FilteredUserDto>? Users { get; set; }
    public int TotalNumberOfUsers { get; set; }
    public int TotalNumberOfPages { get; set; }
}
