using FitTrack.Data.Access;
using FitTrack.Data.Access.Data;
using FitTrack.Data.Object.Enums;
using FitTrack.Test.Unit.Helpers;

namespace FitTrack.Test.Unit.RepositoryTests;

public class UserPreferenceRepositoryTests : RepositoryTestBase
{
    private UserPreferenceRepository CreateRepository(FitTrackContext context)
        => new UserPreferenceRepository(context);

    [Fact(DisplayName = "CreateUserPreferenceAsync adds new UserPreference")]
    public async Task CreateUserPreferenceAsync_WhenCalled_AddsUserPreference()
    {
        // Arrange
        var userPreference = TestHelpers.CreateUserPreference();

        await using var context = new FitTrackContext(_contextOptions);
        var repository = CreateRepository(context);

        // Act
        await repository.CreateUserPreferenceAsync(userPreference);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.UserPreferences.FindAsync(userPreference.Id);
        Assert.NotNull(saved);
        Assert.Equal(userPreference.UnitSystem, saved.UnitSystem);
    }

    [Fact(DisplayName = "UpdateUserPreferenceAsync updates existing UserPreference")]
    public async Task UpdateUserPreferenceAsync_WhenUserPreferenceExists_UpdatesUserPreference()
    {
        // Arrange
        var userPreference = TestHelpers.CreateUserPreference(appTheme: AppThemeEnum.Light);

        await using var context = new FitTrackContext(_contextOptions);
        await context.UserPreferences.AddAsync(userPreference);
        await context.SaveChangesAsync();

        var repository = CreateRepository(context);

        // Act
        userPreference.AppTheme = AppThemeEnum.Dark;
        await repository.UpdateUserPreferenceAsync(userPreference);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.UserPreferences.FindAsync(userPreference.Id);
        Assert.Equal(AppThemeEnum.Dark, updated!.AppTheme);
    }
}
