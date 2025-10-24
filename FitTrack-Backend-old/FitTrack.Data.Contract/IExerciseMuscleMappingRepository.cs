using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IExerciseMuscleMappingRepository
{
    public Task CreatePrimaryMuscleMappingsAsync(List<ExercisePrimaryMuscleMapping> mappings);
    public Task CreateSecondaryMuscleMappingsAsync(List<ExerciseSecondaryMuscleMapping> mappings);
    public Task DeletePrimaryMuscleMappingsAsync(List<ExercisePrimaryMuscleMapping> mappings);
    public Task DeleteSecondaryMuscleMappingsAscyn(List<ExerciseSecondaryMuscleMapping> mappings);
}
