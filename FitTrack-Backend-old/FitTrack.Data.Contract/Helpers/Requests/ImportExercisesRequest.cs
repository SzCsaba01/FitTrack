using Microsoft.AspNetCore.Http;

namespace FitTrack.Data.Contract.Helpers.Requests;

public class ImportExercisesRequest
{
    public required IFormFile ZipFile { get; set; }
}
