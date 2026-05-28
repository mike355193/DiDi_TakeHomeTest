using DiDi.Enums;

namespace DiDi.Dtos;

/// <summary>
/// 用戶資料的 JSON response body。
/// </summary>
/// <param name="Id">用戶 ID。</param>
/// <param name="Email">電子郵件。</param>
/// <param name="Name">姓名。</param>
/// <param name="Age">年齡。</param>
/// <param name="Gender">性別。</param>
/// <param name="Province">所在省份。</param>
/// <param name="City">所在城市。</param>
public sealed record UserResponse(
    int Id,
    string Email,
    string Name,
    int? Age,
    Gender Gender,
    string Province,
    string City);
