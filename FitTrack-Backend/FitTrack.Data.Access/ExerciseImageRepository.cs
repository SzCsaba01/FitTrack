using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Access;
// TEST:
public class ExerciseImageRepository : IExerciseImageRepository
{
    private readonly FitTrackContext _context;

    public ExerciseImageRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task CreateExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages)
    {
        await _context.AddRangeAsync(exerciseImages);
    }

    public Task UpdateExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages)
    {
        _context.UpdateRange(exerciseImages);

        return Task.CompletedTask;
    }

    public Task DeleteExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages)
    {
        _context.RemoveRange(exerciseImages);

        return Task.CompletedTask;
    }
}
