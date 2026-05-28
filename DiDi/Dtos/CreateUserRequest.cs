using DiDi.Enums;

namespace DiDi.Dtos;

/// <summary>
/// 創建用戶的 JSON request body。
/// </summary>
/// <param name="Email">電子郵件，必須為有效格式，且系統內不能重複。</param>
/// <param name="Password">密碼，長度不少於 6，必須為數字和英文字母混合，至少有一個數字和一個英文字母。</param>
/// <param name="Name">姓名，必填。</param>
/// <param name="Age">年齡，可選。</param>
/// <param name="Gender">性別，請填入「男」或「女」。</param>
/// <param name="Province">所在地區的省份，必須符合規格書地區可選項。</param>
/// <param name="City">所在地區的城市，必須符合所選省份底下的城市。</param>
public sealed record CreateUserRequest(
    string Email,
    string Password,
    string Name,
    int? Age,
    Gender Gender,
    string Province,
    string City);
