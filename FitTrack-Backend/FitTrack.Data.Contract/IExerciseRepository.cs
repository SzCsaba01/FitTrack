using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IExerciseRepository
{
    public Task<Dictionary<string, ExerciseEntity>> GetAllExercisesAsDictionaryAsync();
    public Task CreateExercisesAsync(ICollection<ExerciseEntity> exercises);
    public Task UpdateExercisesAsync(ICollection<ExerciseEntity> exercises);
    public Task DeleteExercise(ExerciseEntity exercise);
}
