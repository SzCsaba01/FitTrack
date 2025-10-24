using Microsoft.EntityFrameworkCore.Storage;

namespace FitTrack.Data.Contract;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task CommitTransactionAsync(IDbContextTransaction transaction);
    public Task RollbackTransactionAsync(IDbContextTransaction transaction);
}
