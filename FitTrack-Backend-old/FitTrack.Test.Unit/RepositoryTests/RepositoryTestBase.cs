using FitTrack.Data.Access.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Test.Unit.RepositoryTests;

public class RepositoryTestBase : IDisposable
{
    protected readonly DbContextOptions<FitTrackContext> _contextOptions;

    public RepositoryTestBase()
    {
        _contextOptions = new DbContextOptionsBuilder<FitTrackContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    public void Dispose()
    {
        using var context = new FitTrackContext(_contextOptions);
        context.Database.EnsureDeleted();
    }
}
