using FitTrack.Data.Access;
using FitTrack.Data.Access.Data;
using FitTrack.Test.Unit.Helpers;

namespace FitTrack.Test.Unit.RepositoryTests;

public class UserProfileRepositoryTests : RepositoryTestBase
{
    private UserProfileRepository CreateRepository(FitTrackContext context)
        => new UserProfileRepository(context);

    [Fact(DisplayName = "CreateUserProfileAsync adds a new UserProfile")]
    public async Task CreateUserProfileAsync_WhenCalled_AddsUserProfile()
    {
        // Arrange
        var userProfile = TestHelpers.CreateUserProfile();
        await using var context = new FitTrackContext(_contextOptions);
        var repository = CreateRepository(context);

        // Act
        await repository.CreateUserProfileAsync(userProfile);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.UserProfiles.FindAsync(userProfile.Id);
        Assert.NotNull(saved);
        Assert.Equal(userProfile.FirstName, saved.FirstName);
    }

    [Fact(DisplayName = "UpdateUserProfileAsync updates existing UserProfile")]
    public async Task UpdateUserProfileAsync_WhenUserProfileExists_UpdatesUserProfile()
    {
        // Arrange
        var userProfile = TestHelpers.CreateUserProfile(firstName: "OriginalName");
        await using var context = new FitTrackContext(_contextOptions);
        await context.UserProfiles.AddAsync(userProfile);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        userProfile.FirstName = "UpdatedName";
        await repository.UpdateUserProfileAsync(userProfile);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.UserProfiles.FindAsync(userProfile.Id);
        Assert.Equal("UpdatedName", updated.FirstName);
    }
}
