using Microsoft.AspNetCore.Http;

namespace FitTrack.Data.Contract;

public class ImportFoodRequest
{
    public required IFormFile FoodJsonFile { get; set; }
}
