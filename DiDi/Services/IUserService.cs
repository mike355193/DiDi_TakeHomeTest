using DiDi.Dtos;
using DiDi.Enums;

namespace DiDi.Services;

public interface IUserService
{
    Task<CreateUserResult> CreateAsync(CreateUserRequest request);

    Task<UserResponse?> GetByIdAsync(int id);

    Task<PagedResult<UserResponse>> SearchAsync(string? name, int? minAge, int? maxAge, Gender? gender, int page);

    Task<IReadOnlyList<UserSummaryResponse>> GetSummaryAsync();

    IReadOnlyList<RegionOption> GetRegions();
}
