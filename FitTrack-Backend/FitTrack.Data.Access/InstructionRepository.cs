using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Access;
// TEST:
public class InstructionRepository : IInstructionRepository
{
    private readonly FitTrackContext _context;

    public InstructionRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task CreateInscrutionsAsync(List<InstructionEntity> instructions)
    {
        await _context.Instructions.AddRangeAsync(instructions);
    }

    public Task DeleteInstructionsAsync(List<InstructionEntity> instructions)
    {
        _context.Instructions.RemoveRange(instructions);

        return Task.CompletedTask;
    }
}
