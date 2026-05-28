using DiDi.Enums;

namespace DiDi.Dtos;

/// <summary>
/// Internal search criteria for querying users.
/// </summary>
/// <param name="Name">Optional fuzzy name keyword.</param>
/// <param name="MinAge">Optional minimum age.</param>
/// <param name="MaxAge">Optional maximum age.</param>
/// <param name="Gender">性別搜尋條件，請填入「男」或「女」。</param>
/// <param name="Page">Page number.</param>
/// <param name="PageSize">Number of records per page. The API uses 10.</param>
public sealed record UserSearchRequest(
    string? Name,
    int? MinAge,
    int? MaxAge,
    Gender? Gender,
    int Page = 1,
    int PageSize = 10);
