using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IExerciseImageRepository
{
    public Task CreateExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages);
    public Task UpdateExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages);
    public Task DeleteExerciseImagesAsync(ICollection<ExerciseImageEntity> exerciseImages);
}
