using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using Microsoft.EntityFrameworkCore.Storage;

namespace FitTrack.Data.Access;

public class UnitOfWork : IUnitOfWork
{
    private readonly FitTrackContext _context;

    public UnitOfWork(FitTrackContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync()
        => _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
        await transaction.DisposeAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
        await transaction.DisposeAsync();
    }
}
