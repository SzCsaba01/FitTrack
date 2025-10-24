using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;
public interface IMuscleRepository
{
    public Task<Dictionary<string, MuscleEntity>> GetMusclesAsDictonaryAsync();
    public Task CreateMusclesAsync(List<MuscleEntity> muscles);
    public Task DeleteMusclesAsync(List<MuscleEntity> muscles);

}
