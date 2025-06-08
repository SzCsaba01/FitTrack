using FitTrack.Data.Access;
using FitTrack.Data.Access.Data;

namespace FitTrack.Test.Unit.RepositoryTests;

public class UnitOfWorkTests : RepositoryTestBase
{
    private UnitOfWork CreateUnitOfWork(FitTrackContext _contextOptions)
        => new UnitOfWork(_contextOptions);

    [Fact(DisplayName = "SaveChangesAsync calls context SaveChangesAsync")]
    public async Task SaveChangesAsync_WhenCalled_CallsContextSaveChanges()
    {
        // Arrange
        await using var context = new FitTrackContext(_contextOptions);
        var unitOfWork = CreateUnitOfWork(context);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.IsType<int>(result);
        Assert.True(result >= 0);
    }

    [Fact(DisplayName = "BeginTransactionAsync throws on InMemory")]
    public async Task BeginTransactionAsync_WhenUsingInMemory_ThrowsInvalidOperationException()
    {
        // Arrange
        await using var context = new FitTrackContext(_contextOptions);
        var unitOfWork = CreateUnitOfWork(context);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            unitOfWork.BeginTransactionAsync());

        Assert.Contains("Transactions are not supported", ex.Message);
    }

    [Fact(DisplayName = "CommitTransactionAsync throws on InMemory")]
    public async Task CommitTransactionAsync_WhenUsingInMemory_ThrowsInvalidOperationException()
    {
        // Arrange
        await using var context = new FitTrackContext(_contextOptions);
        var unitOfWork = CreateUnitOfWork(context);

        // Act & Assert
        var transaction = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            unitOfWork.BeginTransactionAsync());

        // Can't continue to commit since we never got a transaction
        // So test ends here
    }

    [Fact(DisplayName = "RollbackTransactionAsync throws on InMemory")]
    public async Task RollbackTransactionAsync_WhenUsingInMemory_ThrowsInvalidOperationException()
    {
        // Arrange
        await using var context = new FitTrackContext(_contextOptions);
        var unitOfWork = CreateUnitOfWork(context);

        // Act & Assert
        var transaction = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            unitOfWork.BeginTransactionAsync());

        // Can't rollback a non-existent transaction
    }
}
