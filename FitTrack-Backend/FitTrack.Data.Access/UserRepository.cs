using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Object.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data.Access;

public class UserRepository : IUserRepository
{
    private readonly FitTrackContext _context;

    public UserRepository(FitTrackContext context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Where(u => u.Username == username)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByUsernameOrEmailAsync(string credential)
    {
        return await _context.Users
            .Where(u => u.Username == credential || u.Email == credential)
            .Include(u => u.Role)
                .ThenInclude(u => u.PermissionMappings)
                    .ThenInclude(u => u.Permission)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByRefreshTokenAsync(byte[] refreshToken)
    {
        return await _context.Users
            .Where(u => u.RefreshToken.SequenceEqual(refreshToken))
            .Include(u => u.Role)
                .ThenInclude(u => u.PermissionMappings)
                    .ThenInclude(u => u.Permission)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByEmailVerificationTokenAsync(byte[] emailVerificationToken)
    {
        return await _context.Users
            .Where(u => u.EmailVerificationToken.SequenceEqual(emailVerificationToken))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByChangePasswordTokenAsync(byte[] changePasswordToken)
    {
        return await _context.Users
            .Where(u => u.ChangePasswordToken.SequenceEqual(changePasswordToken))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }


    public async Task CreateUserAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task UpdateUserAsync(UserEntity user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteUserAsync(UserEntity User)
    {
        _context.Users.Remove(User);
        return Task.CompletedTask;
    }
}
