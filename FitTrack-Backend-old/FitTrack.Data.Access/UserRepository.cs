using FitTrack.Data.Access.Data;
using FitTrack.Data.Contract;
using FitTrack.Data.Contract.Helpers.Requests;
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
                .ThenInclude(u => u!.PermissionMappings)
                    .ThenInclude(u => u.Permission)
            .Include(u => u.UserPreference)
            .Include(u => u.UserProfile)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByRefreshTokenAsync(byte[] refreshToken)
    {
        return await _context.Users
            .Where(u => u.RefreshToken != null && u.RefreshToken.SequenceEqual(refreshToken))
            .Include(u => u.Role)
                .ThenInclude(u => u!.PermissionMappings)
                    .ThenInclude(u => u.Permission)
            .Include(u => u.UserProfile)
            .Include(u => u.UserPreference)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByEmailVerificationTokenAsync(byte[] emailVerificationToken)
    {
        return await _context.Users
            .Where(u => u.EmailVerificationToken != null && u.EmailVerificationToken.SequenceEqual(emailVerificationToken))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetUserByChangePasswordTokenAsync(byte[] changePasswordToken)
    {
        return await _context.Users
            .Where(u => u.ChangePasswordToken != null && u.ChangePasswordToken.SequenceEqual(changePasswordToken))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    // TEST:
    public async Task<(List<UserEntity> users, int totalNumberOfUsers)> GetFilteredUsersAsync(GetFilteredUsersRequest request)
    {
        var query = _context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.Role)
            .AsNoTracking()
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(request.Search))
        {
            query = query
                .Where(u => u.Username.ToLower().Contains(request.Search) ||
                            u.Email.ToLower().Contains(request.Search) ||
                            u.UserProfile.FirstName.ToLower().Contains(request.Search) ||
                            u.UserProfile.LastName.ToLower().Contains(request.Search));
        }

        var totalNumberOfUsers = await query.CountAsync();
        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (users, totalNumberOfUsers);
    }

    // TEST:
    public async Task<UserEntity?> GetUserWithDetailsByIdAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.UserProfile)
            .Include(u => u.Role)
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

    public Task DeleteUserAsync(UserEntity user)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }
}
