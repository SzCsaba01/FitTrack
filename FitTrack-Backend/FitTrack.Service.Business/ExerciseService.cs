using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers;
using FitTrack.Data.Contract.Helpers.DTOs;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;
using FitTrack.Data.Object.Enums;
using FitTrack.Service.Contract;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text.Json;

namespace FitTrack.Service.Business;
// TEST: 
public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseMuscleMappingRepository _exerciseMuscleMappingRepository;
    private readonly IMuscleRepository _muscleRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ILogger<ExerciseService> _logger;

    public ExerciseService(
        IExerciseRepository exerciseRepository,
        IExerciseMuscleMappingRepository exerciseMuscleMappingRepository,
        IMuscleRepository muscleRepository,
        IEquipmentRepository equipmentRepository,
        IUnitOfWork unitOfWork,
        ICloudinaryService cloudinaryService,
        ILogger<ExerciseService> logger)
    {
        _exerciseRepository = exerciseRepository;
        _exerciseMuscleMappingRepository = exerciseMuscleMappingRepository;
        _muscleRepository = muscleRepository;
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
        _logger = logger;
    }

    public async Task ImportExercisesFromZipAsync(ImportExercisesRequest request)
    {
        var tempFilePath = Path.GetTempFileName();
        await using (var tempFileStream = new FileStream(
            tempFilePath,
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.None,
            bufferSize: 8192,
            FileOptions.DeleteOnClose))
        {
            await request.ZipFile.CopyToAsync(tempFileStream).ConfigureAwait(false);
            tempFileStream.Position = 0;

            using var archive = new ZipArchive(tempFileStream, ZipArchiveMode.Read);

            var jsonEntry = archive.Entries
                .FirstOrDefault(e => e.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase));

            if (jsonEntry == null)
            {
                _logger.LogError("No JSON file found in ZIP!");
                throw new ValidationException("No JSON file found in ZIP!");
            }

            List<ExerciseImportDto> importedDtos;
            await using (var jsonStream = jsonEntry.Open())
            {
                importedDtos = await JsonSerializer.DeserializeAsync<List<ExerciseImportDto>>(jsonStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false)
                    ?? new List<ExerciseImportDto>();
            }

            if (!importedDtos.Any())
            {
                _logger.LogWarning("No exercises found in JSON!");
                return;
            }

            var existingExercises = await _exerciseRepository.GetAllExercisesAsDictionaryAsync().ConfigureAwait(false);
            var existingMuscles = await _muscleRepository.GetMusclesAsDictonaryAsync().ConfigureAwait(false);
            var existingEquipment = await _equipmentRepository.GetEquipmentsAsDictonaryAsync().ConfigureAwait(false);

            var exercisesToAdd = new List<ExerciseEntity>();
            var exercisesToUpdate = new List<ExerciseEntity>();
            var exercisePrimaryMusclesToAdd = new List<ExercisePrimaryMuscleMapping>();
            var exerciseSecondaryMusclesToAdd = new List<ExerciseSecondaryMuscleMapping>();
            var exercisePrimaryMusclesToRemove = new List<ExercisePrimaryMuscleMapping>();
            var exerciseSecondaryMusclesToRemove = new List<ExerciseSecondaryMuscleMapping>();
            var musclesToAdd = new List<MuscleEntity>();
            var equipmentsToAdd = new List<EquipmentEntity>();

            var groupedImages = archive.Entries
                .Where(e => !e.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(e.Name))
                .GroupBy(e => Path.GetFileName(Path.GetDirectoryName(e.FullName)))
                .Where(g => g.Key != null)
                .ToDictionary(g => g.Key!, g => g.ToList());

            foreach (var dto in importedDtos)
            {
                if (!existingExercises.TryGetValue(dto.Id, out var entity))
                {
                    entity = new ExerciseEntity
                    {
                        Name = dto.Name,
                        ExternalId = dto.Id,
                        Instructions = new List<InstructionEntity>(),
                        PrimaryMuscles = new List<ExercisePrimaryMuscleMapping>(),
                        SecondaryMuscles = new List<ExerciseSecondaryMuscleMapping>(),
                        Images = new List<ExerciseImageEntity>()
                    };
                    exercisesToAdd.Add(entity);
                }
                else
                {
                    exercisesToUpdate.Add(entity);
                    entity.Instructions!.Clear();
                    entity.Images!.Clear();
                }

                await MapImportExerciseToEntity(
                    dto,
                    entity,
                    existingMuscles,
                    exercisePrimaryMusclesToAdd,
                    exerciseSecondaryMusclesToRemove,
                    exercisePrimaryMusclesToRemove,
                    exerciseSecondaryMusclesToRemove,
                    existingEquipment,
                    groupedImages,
                    musclesToAdd,
                    equipmentsToAdd).ConfigureAwait(false);
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (musclesToAdd.Any())
                {
                    await _muscleRepository.CreateMusclesAsync(musclesToAdd).ConfigureAwait(false);
                }

                if (equipmentsToAdd.Any())
                {
                    await _equipmentRepository.CreateEquipmentsAsync(equipmentsToAdd).ConfigureAwait(false);
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                if (exercisesToAdd.Any())
                {
                    await _exerciseRepository.CreateExercisesAsync(exercisesToAdd).ConfigureAwait(false);
                }

                if (exercisesToUpdate.Any())
                {
                    await _exerciseRepository.UpdateExercisesAsync(exercisesToUpdate).ConfigureAwait(false);
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                await _unitOfWork.CommitTransactionAsync(transaction).ConfigureAwait(false);

                _logger.LogInformation("Successfully imported {Count} exercises.", importedDtos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during importing exercises");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private async Task MapImportExerciseToEntity(
        ExerciseImportDto dto,
        ExerciseEntity exercise,
        Dictionary<string, MuscleEntity> existingMuscles,
        List<ExercisePrimaryMuscleMapping> exercisePrimaryMusclesToAdd,
        List<ExerciseSecondaryMuscleMapping> exerciseSecondaryMusclesToAdd,
        List<ExercisePrimaryMuscleMapping> exercisePrimaryMusclesToRemove,
        List<ExerciseSecondaryMuscleMapping> exerciseSecondaryMusclesToRemove,
        Dictionary<string, EquipmentEntity> existingEquipments,
        Dictionary<string, List<ZipArchiveEntry>> groupedImages,
        List<MuscleEntity> musclesToAdd,
        List<EquipmentEntity> equipmentsToAdd)
    {
        exercise.Name = dto.Name;
        exercise.Force = HelperMethods.ParseEnum<ForceEnum>(dto.Force!);
        exercise.Mechanic = HelperMethods.ParseEnum<MechanicEnum>(dto.Mechanic!);
        exercise.Difficulty = HelperMethods.ParseEnum<DifficultyEnum>(dto.Level);
        exercise.Category = HelperMethods.ParseEnum<ExerciseCategoryEnum>(dto.Category);

        if (!string.IsNullOrWhiteSpace(dto.Equipment))
        {
            if (!existingEquipments.TryGetValue(dto.Equipment, out var equipment))
            {
                equipment = new EquipmentEntity { Name = dto.Equipment };
                equipmentsToAdd.Add(equipment);
                existingEquipments[dto.Equipment] = equipment;
            }
            exercise.Equipment = equipment;
        }

        foreach (var instruction in dto.Instructions ?? Enumerable.Empty<string>())
        {
            exercise.Instructions!.Add(new InstructionEntity { Instruction = instruction });
        }

        var currentPrimaryIds = exercise.PrimaryMuscles!.Select(m => m.MuscleId).ToHashSet();
        var newPrimaryIds = dto.PrimaryMuscles!
            .Select(name =>
            {
                if (!existingMuscles.TryGetValue(name, out var muscle))
                {
                    muscle = new MuscleEntity { Name = name };
                    musclesToAdd.Add(muscle);
                    existingMuscles[name] = muscle;
                }
                return muscle.Id;
            })
            .ToHashSet();

        var toRemovePrimary = exercise.PrimaryMuscles!
            .Where(pm => !newPrimaryIds.Contains(pm.MuscleId))
            .ToList();
        exercisePrimaryMusclesToRemove.AddRange(toRemovePrimary);

        var toAddPrimary = newPrimaryIds
            .Where(id => !currentPrimaryIds.Contains(id))
            .Select(id => new ExercisePrimaryMuscleMapping { Exercise = exercise, MuscleId = id })
            .ToList();
        exercisePrimaryMusclesToAdd.AddRange(toAddPrimary);

        var currentSecondaryIds = exercise.SecondaryMuscles!.Select(m => m.MuscleId).ToHashSet();
        var newSecondaryIds = dto.SecondaryMuscles!
            .Select(name =>
            {
                if (!existingMuscles.TryGetValue(name, out var muscle))
                {
                    muscle = new MuscleEntity { Name = name };
                    musclesToAdd.Add(muscle);
                    existingMuscles[name] = muscle;
                }
                return muscle.Id;
            })
            .ToHashSet();

        var toRemoveSecondary = exercise.SecondaryMuscles!
            .Where(sm => !newSecondaryIds.Contains(sm.MuscleId))
            .ToList();
        exerciseSecondaryMusclesToRemove.AddRange(toRemoveSecondary);

        var toAddSecondary = newSecondaryIds
            .Where(id => !currentSecondaryIds.Contains(id))
            .Select(id => new ExerciseSecondaryMuscleMapping { Exercise = exercise, MuscleId = id })
            .ToList();
        exerciseSecondaryMusclesToAdd.AddRange(toAddSecondary);

        // if (groupedImages.TryGetValue(dto.Id, out var entries))
        // {
        //     var uploadTasks = entries.Select(async entry =>
        //     {
        //         await using var stream = entry.Open();
        //         var fileName = Path.GetFileName(entry.FullName);
        //         var folder = $"{AppConstants.EXERCISE_IMAGES_FOLDER}/{dto.Id}";
        //         return await _cloudinaryService.UploadImageAsync(stream, fileName, folder);
        //     }).ToList();
        //
        //     var urls = await Task.WhenAll(uploadTasks);
        //
        //     var exerciseImageEntities = urls
        //         .Where(url => !String.IsNullOrEmpty(url))
        //         .Select(url => new ExerciseImageEntity { ImageUrl = url });
        //
        //     foreach (var exerciseImageEntity in exerciseImageEntities)
        //     {
        //         entity.Images.Add(exerciseImageEntity);
        //     }
        // }
        exercise.Images!.Add(new ExerciseImageEntity { ImageUrl = "test" });
    }
}
