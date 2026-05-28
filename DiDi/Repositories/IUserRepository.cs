using DiDi.Dtos;
using DiDi.Entities;

namespace DiDi.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);

    Task<User> CreateAsync(User user);

    Task<UserResponse?> GetByIdAsync(int id);

    Task<PagedResult<UserResponse>> SearchAsync(UserSearchRequest request);

    Task<IReadOnlyList<UserSummaryResponse>> GetSummaryAsync();
}
