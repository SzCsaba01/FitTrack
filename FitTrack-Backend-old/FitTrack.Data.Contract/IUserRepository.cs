using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract;

public interface IUserRepository
{
    public Task<UserEntity?> GetUserByIdAsync(Guid userId);
    public Task<UserEntity?> GetUserByUsernameOrEmailAsync(string credential);
    public Task<UserEntity?> GetUserByUsernameAsync(string username);
    public Task<UserEntity?> GetUserByEmailAsync(string email);
    public Task<UserEntity?> GetUserByRefreshTokenAsync(byte[] refreshToken);
    public Task<UserEntity?> GetUserByEmailVerificationTokenAsync(byte[] emailVerificationToken);
    public Task<UserEntity?> GetUserByChangePasswordTokenAsync(byte[] changePasswordToken);
    public Task<UserEntity?> GetUserWithDetailsByIdAsync(Guid userId);
    public Task<(List<UserEntity> users, int totalNumberOfUsers)> GetFilteredUsersAsync(GetFilteredUsersRequest request);
    public Task CreateUserAsync(UserEntity user);
    public Task UpdateUserAsync(UserEntity user);
    public Task DeleteUserAsync(UserEntity user);
}
