using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;
// TEST:
public class FoodRepository : IFoodRepository
{
    private readonly FitTrackContext _context;

    public FoodRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, FoodEntity>> GetAllFoodsAsDictionaryAsync()
    {
        return await _context.Foods
            .Include(f => f.Category)
            .Include(f => f.Nutritions)
            .ToDictionaryAsync(
                    f => f.Name,
                    f => f
            );
    }

    public async Task CreateFoodsAsync(ICollection<FoodEntity> foods)
    {
        await _context.Foods.AddRangeAsync(foods);
    }

    public Task UpdateFoodsAsync(ICollection<FoodEntity> foods)
    {
        _context.Foods.UpdateRange(foods);
        return Task.CompletedTask;
    }

    public Task DeleteFoodAsync(FoodEntity food)
    {
        _context.Foods.Remove(food);
        return Task.CompletedTask;
    }
}
