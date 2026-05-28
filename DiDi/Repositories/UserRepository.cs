using DiDi.Dtos;
using DiDi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiDi.Repositories;

public sealed class UserRepository(UserManagementDbContext db) : IUserRepository
{
    public Task<bool> EmailExistsAsync(string email) =>
        db.Users.AnyAsync(user => user.Email == email);

    public async Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public Task<UserResponse?> GetByIdAsync(int id) =>
        db.Users
            .AsNoTracking()
            .Where(user => user.Id == id)
            .Select(user => new UserResponse(
                user.Id,
                user.Email,
                user.Name,
                user.Age,
                user.Gender,
                user.Province,
                user.City))
            .FirstOrDefaultAsync();

    public async Task<PagedResult<UserResponse>> SearchAsync(UserSearchRequest request)
    {
        var page = Math.Max(request.Page, 1);
        const int pageSize = 10;
        var query = db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(user => user.Name.Contains(request.Name.Trim()));
        }

        if (request.MinAge.HasValue)
        {
            query = query.Where(user => user.Age >= request.MinAge);
        }

        if (request.MaxAge.HasValue)
        {
            query = query.Where(user => user.Age <= request.MaxAge);
        }

        if (request.Gender.HasValue)
        {
            query = query.Where(user => user.Gender == request.Gender);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(user => user.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new UserResponse(
                user.Id,
                user.Email,
                user.Name,
                user.Age,
                user.Gender,
                user.Province,
                user.City))
            .ToListAsync();

        return new PagedResult<UserResponse>(
            items,
            page,
            pageSize,
            totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public async Task<IReadOnlyList<UserSummaryResponse>> GetSummaryAsync() =>
        await db.Users
            .AsNoTracking()
            .GroupBy(user => new { user.City, user.Gender })
            .OrderBy(group => group.Key.City)
            .ThenBy(group => group.Key.Gender)
            .Select(group => new UserSummaryResponse(group.Key.City, group.Key.Gender, group.Count()))
            .ToListAsync();
}
