using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IEquipmentRepository
{
    public Task<Dictionary<string, EquipmentEntity>> GetEquipmentsAsDictonaryAsync();
    public Task CreateEquipmentsAsync(List<EquipmentEntity> equipments);
    public Task DeleteEquipmentsAsync(List<EquipmentEntity> equipments);
}
