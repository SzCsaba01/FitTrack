using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;
// TEST: 
public class EquipmentRepository : IEquipmentRepository
{
    private readonly FitTrackContext _context;

    public EquipmentRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, EquipmentEntity>> GetEquipmentsAsDictonaryAsync()
    {
        return await _context.Equipments
            .AsNoTracking()
            .ToDictionaryAsync(
                e => e.Name,
                e => e
            );
    }

    public async Task CreateEquipmentsAsync(List<EquipmentEntity> equipments)
    {
        await _context.Equipments.AddRangeAsync(equipments);
    }

    public Task DeleteEquipmentsAsync(List<EquipmentEntity> equipments)
    {
        _context.Equipments.RemoveRange(equipments);

        return Task.CompletedTask;
    }
}
