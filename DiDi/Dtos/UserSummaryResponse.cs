using DiDi.Enums;

namespace DiDi.Dtos;

/// <summary>
/// 用戶數據匯總查詢的 JSON response body。
/// </summary>
/// <param name="City">所在城市。</param>
/// <param name="Gender">性別。</param>
/// <param name="TotalCount">總人數。</param>
public sealed record UserSummaryResponse(
    string City,
    Gender Gender,
    int TotalCount);
