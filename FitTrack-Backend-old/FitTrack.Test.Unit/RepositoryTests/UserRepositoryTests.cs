using FitTrack.Data.Access;
using FitTrack.Data.Access.Data;
using FitTrack.Test.Unit.Helpers;

namespace FitTrack.Test.Unit.RepositoryTests;

public class UserRepositoryTests : RepositoryTestBase
{
    private UserRepository CreateRepository(FitTrackContext context)
        => new UserRepository(context);

    [Fact(DisplayName = "GetUserByIdAsync returns user when exists")]
    public async Task GetUserByIdAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        await using var context = new FitTrackContext(_contextOptions);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact(DisplayName = "GetUserByEmailAsync returns user when exists")]
    public async Task GetUserByEmailAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var user = TestHelpers.CreateUser(email: "email@example.com");
        await using var context = new FitTrackContext(_contextOptions);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetUserByEmailAsync("email@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact(DisplayName = "GetUserByUsernameAsync returns user when exists")]
    public async Task GetUserByUsernameAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var user = TestHelpers.CreateUser(username: "username1");
        await using var context = new FitTrackContext(_contextOptions);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetUserByUsernameAsync("username1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact(DisplayName = "CreateUserAsync adds user")]
    public async Task CreateUserAsync_ValidUser_AddsUserToDatabase()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        await using var context = new FitTrackContext(_contextOptions);
        var repository = CreateRepository(context);

        // Act
        await repository.CreateUserAsync(user);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.Users.FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal(user.Username, saved.Username);
    }

    [Fact(DisplayName = "UpdateUserAsync updates user")]
    public async Task UpdateUserAsync_ValidChanges_UpdatesUserInDatabase()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        await using var context = new FitTrackContext(_contextOptions);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        user.Username = "updatedUsername";
        await repository.UpdateUserAsync(user);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.Users.FindAsync(user.Id);
        Assert.Equal("updatedUsername", updated!.Username);
    }

    [Fact(DisplayName = "DeleteUserAsync deletes user")]
    public async Task DeleteUserAsync_ValidUser_DeletesUserFromDatabase()
    {
        // Arrange
        var user = TestHelpers.CreateUser();
        await using var context = new FitTrackContext(_contextOptions);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        await repository.DeleteUserAsync(user);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await context.Users.FindAsync(user.Id);
        Assert.Null(deleted);
    }
}
