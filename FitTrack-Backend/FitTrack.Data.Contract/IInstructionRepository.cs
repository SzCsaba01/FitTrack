using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IInstructionRepository
{
    public Task CreateInscrutionsAsync(List<InstructionEntity> instructions);
    public Task DeleteInstructionsAsync(List<InstructionEntity> instructions);
}
