using FitTrack.Data.Access;
using FitTrack.Data.Access.Data;
using FitTrack.Data.Object.Enums;
using FitTrack.Test.Unit.Helpers;

namespace FitTrack.Test.Unit.RepositoryTests;

public class RoleRepositoryTests : RepositoryTestBase
{
    private RoleRepository CreateRepository() => new(new FitTrackContext(_contextOptions));

    [Fact(DisplayName = "GetRoleByNameAsync returns role with permissions when exists")]
    public async Task GetRoleByNameAsync_WhenRoleExists_ReturnsRoleWithPermissions()
    {
        // Arrange
        var role = TestHelpers.CreateRole(RoleEnum.Admin);

        await using var context = new FitTrackContext(_contextOptions);
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var repository = CreateRepository();

        // Act
        var result = await repository.GetRoleByNameAsync(RoleEnum.Admin);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(role.Id, result.Id);
        Assert.Equal(RoleEnum.Admin, result.RoleName);
    }

    [Fact(DisplayName = "GetRoleByNameAsync returns null when role does not exist")]
    public async Task GetRoleByNameAsync_WhenRoleDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var result = await repository.GetRoleByNameAsync(RoleEnum.User);

        // Assert
        Assert.Null(result);
    }
}
