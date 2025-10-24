using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IFoodRepository
{
    public Task<Dictionary<string, FoodEntity>> GetAllFoodsAsDictionaryAsync();
    public Task CreateFoodsAsync(ICollection<FoodEntity> foods);
    public Task UpdateFoodsAsync(ICollection<FoodEntity> foods);
    public Task DeleteFoodAsync(FoodEntity food);
}
