using System.Security.Cryptography;
using System.Text;
using DiDi.Dtos;
using DiDi.Entities;
using DiDi.Enums;
using DiDi.Repositories;
using DiDi.Validation;

namespace DiDi.Services;

public sealed class UserService(IUserRepository users) : IUserService
{
    public async Task<CreateUserResult> CreateAsync(CreateUserRequest request)
    {
        var errors = UserValidator.ValidateCreateUser(request);
        if (errors.Count > 0)
        {
            return CreateUserResult.ValidationFailed(errors);
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await users.EmailExistsAsync(normalizedEmail))
        {
            return CreateUserResult.DuplicateEmail();
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = HashPassword(request.Password),
            Name = request.Name.Trim(),
            Age = request.Age,
            Gender = request.Gender,
            Province = request.Province,
            City = request.City
        };

        var created = await users.CreateAsync(user);
        return CreateUserResult.Success(ToResponse(created));
    }

    public Task<PagedResult<UserResponse>> SearchAsync(
        string? name,
        int? minAge,
        int? maxAge,
        Gender? gender,
        int page) =>
        users.SearchAsync(new UserSearchRequest(name, minAge, maxAge, gender, page, 10));

    public Task<UserResponse?> GetByIdAsync(int id) =>
        users.GetByIdAsync(id);

    public Task<IReadOnlyList<UserSummaryResponse>> GetSummaryAsync() =>
        users.GetSummaryAsync();

    public IReadOnlyList<RegionOption> GetRegions() =>
        UserValidator.ValidRegions
            .Select(region => new RegionOption(region.Key, region.Value.Order().ToList()))
            .ToList();

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.Email, user.Name, user.Age, user.Gender, user.Province, user.City);

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
