using DiDi.Dtos;

namespace DiDi.Services;

public sealed record CreateUserResult(
    bool IsSuccess,
    UserResponse? User,
    IReadOnlyList<string> Errors,
    bool EmailAlreadyExists)
{
    public static CreateUserResult Success(UserResponse user) =>
        new(true, user, [], false);

    public static CreateUserResult ValidationFailed(IReadOnlyList<string> errors) =>
        new(false, null, errors, false);

    public static CreateUserResult DuplicateEmail() =>
        new(false, null, [], true);
}
