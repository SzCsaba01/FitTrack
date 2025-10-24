using FitTrack.Data.Contract;
using FitTrack.Service.Contract;
using Microsoft.Extensions.Logging;

namespace FitTrack.Service.Business;
// TEST:
public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FoodService> _logger;

    public Task ImportFoodsFromJsonFileAsync(ImportFoodRequest request)
    {
        return Task.CompletedTask;
    }
}
