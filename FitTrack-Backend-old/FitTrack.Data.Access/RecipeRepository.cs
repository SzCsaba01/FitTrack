using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;
// TEST: 
public class RecipeRepository : IRecipeRepository
{
    private readonly FitTrackContext _context;

    public RecipeRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, RecipeEntity>> GetAllRecipesAsDictionaryAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Directions)
            .Include(r => r.CategoryMappings)
            .Include(r => r.Nutritions)
            .ToDictionaryAsync(
                r => r.Name,
                r => r
            );
    }

    public async Task CreateRecipesAsync(ICollection<RecipeEntity> recipes)
    {
        await _context.Recipes.AddRangeAsync(recipes);
    }

    public Task UpdateRecipesAsync(ICollection<RecipeEntity> recipes)
    {
        _context.Recipes.UpdateRange(recipes);
        return Task.CompletedTask;
    }

    public Task DeleteRecipeAsync(RecipeEntity recipe)
    {
        _context.Recipes.Remove(recipe);
        return Task.CompletedTask;
    }

}
