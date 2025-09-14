using FitTrack.Data.Contract.Helpers.Requests;

namespace FitTrack.Service.Contract;

public interface IExerciseService
{
    public Task ImportExercisesFromZipAsync(ImportExercisesRequest request);
}
