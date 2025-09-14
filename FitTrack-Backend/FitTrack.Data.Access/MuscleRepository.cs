using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;
// TEST: 
public class MuscleRepository : IMuscleRepository
{
    private readonly FitTrackContext _context;

    public MuscleRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, MuscleEntity>> GetMusclesAsDictonaryAsync()
    {
        return await _context.Muscles
            .AsNoTracking()
            .ToDictionaryAsync(
                m => m.Name,
                m => m
            );
    }

    public async Task CreateMusclesAsync(List<MuscleEntity> muscles)
    {
        await _context.Muscles.AddRangeAsync(muscles);
    }

    public Task DeleteMusclesAsync(List<MuscleEntity> muscles)
    {
        _context.Muscles.RemoveRange(muscles);

        return Task.CompletedTask;
    }
}
