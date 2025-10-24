using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IRecipeRepository
{
    public Task<Dictionary<string, RecipeEntity>> GetAllRecipesAsDictionaryAsync();
    public Task CreateRecipesAsync(ICollection<RecipeEntity> recipes);
    public Task UpdateRecipesAsync(ICollection<RecipeEntity> recipes);
    public Task DeleteRecipeAsync(RecipeEntity recipe);
}
