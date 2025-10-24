using FitTrack.Data.Contract;

namespace FitTrack.Service.Contract;

public interface IFoodService
{
    public Task ImportFoodsFromJsonFileAsync(ImportFoodRequest request);
}
