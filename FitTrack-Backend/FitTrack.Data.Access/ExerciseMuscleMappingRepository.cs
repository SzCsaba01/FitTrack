using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Access;

public class ExerciseMuscleMappingRepository : IExerciseMuscleMappingRepository
{
    private readonly FitTrackContext _context;

    public ExerciseMuscleMappingRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task CreatePrimaryMuscleMappingsAsync(List<ExercisePrimaryMuscleMapping> mappings)
    {
        await _context.ExercisePrimaryMuscleMappings.AddRangeAsync(mappings);
    }

    public async Task CreateSecondaryMuscleMappingsAsync(List<ExerciseSecondaryMuscleMapping> mappings)
    {
        await _context.ExerciseSecondaryMuscleMappings.AddRangeAsync(mappings);
    }

    public Task DeletePrimaryMuscleMappingsAsync(List<ExercisePrimaryMuscleMapping> mappings)
    {
        _context.ExercisePrimaryMuscleMappings.RemoveRange(mappings);

        return Task.CompletedTask;
    }

    public Task DeleteSecondaryMuscleMappingsAscyn(List<ExerciseSecondaryMuscleMapping> mappings)
    {
        _context.ExerciseSecondaryMuscleMappings.RemoveRange(mappings);

        return Task.CompletedTask;
    }
}
