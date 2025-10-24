using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;
// TEST:
public class ExerciseRepository : IExerciseRepository
{
    private readonly FitTrackContext _context;

    public ExerciseRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, ExerciseEntity>> GetAllExercisesAsDictionaryAsync()
    {
        return await _context.Exercises
            .Include(e => e.Instructions)
            .Include(e => e.PrimaryMuscles)
            .Include(e => e.SecondaryMuscles)
            .Include(e => e.Images)
            .ToDictionaryAsync(
                e => e.ExternalId ?? e.Id.ToString(),
                e => e
            );
    }

    public async Task CreateExercisesAsync(ICollection<ExerciseEntity> exercises)
    {
        await _context.AddRangeAsync(exercises);
    }

    public Task UpdateExercisesAsync(ICollection<ExerciseEntity> exercises)
    {
        _context.Exercises.UpdateRange(exercises);
        return Task.CompletedTask;
    }

    public Task DeleteExercise(ExerciseEntity exercise)
    {
        _context.Exercises.Remove(exercise);
        return Task.CompletedTask;
    }

}
