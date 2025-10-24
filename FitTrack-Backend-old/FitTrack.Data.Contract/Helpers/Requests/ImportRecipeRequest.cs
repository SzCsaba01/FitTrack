using Microsoft.AspNetCore.Http;

namespace FitTrack.Data.Contract;

public class ImportRecipeRequest
{
    public required IFormFile RecipeJsonFile { get; set; }
}
