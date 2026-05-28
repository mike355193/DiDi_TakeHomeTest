namespace DiDi.Dtos;

/// <summary>
/// 分頁查詢結果。查找用戶 API 固定每頁顯示 10 筆。
/// </summary>
/// <typeparam name="T">分頁資料項目型別。</typeparam>
/// <param name="Items">目前頁面的資料。</param>
/// <param name="Page">目前頁碼。</param>
/// <param name="PageSize">每頁筆數。</param>
/// <param name="TotalCount">符合搜索條件的總筆數。</param>
/// <param name="TotalPages">總頁數。</param>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
