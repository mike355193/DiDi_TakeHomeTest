using DiDi.Dtos;
using DiDi.Enums;
using DiDi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiDi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var users = endpoints.MapGroup("/api/users")
            .WithTags("用戶管理");

        users.MapPost("/", Create)
            .WithName("CreateUser")
            .WithSummary("創建用戶")
            .WithDescription("""
                驗證用戶資料並保存到資料庫。

                Request body 必須為 JSON，欄位包含：電子郵件、密碼、姓名、年齡、性別、所在地區。
                電子郵件必須為有效格式且系統內不能重複；密碼長度不少於 6，且必須為數字和英文字母混合，至少包含一個數字和一個英文字母；姓名必填；年齡可選；性別為男/女；所在地區必須符合API:[/api/regions]地區可選項。
                """)
            .Accepts<CreateUserRequest>("application/json")
            .Produces<UserResponse>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        users.MapGet("/{id:int}", GetById)
            .WithName("GetUserById")
            .WithSummary("取得單一用戶")
            .WithDescription("""
                依用戶 ID 取得單一用戶資料。

                Response 為 JSON 格式，不回傳密碼或密碼雜湊。
                """)
            .Produces<UserResponse>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        users.MapGet("/", Search)
            .WithName("SearchUsers")
            .WithSummary("查找用戶")
            .WithDescription("""
                用戶輸入搜索條件，列出符合條件的用戶記錄，查詢結果需要分頁顯示，每頁顯示 10 筆。

                可選擇的搜索條件包含：姓名、年齡範圍、性別。
                姓名支援模糊查詢，包含關鍵字的記錄都會列出。
                Response 為 JSON 格式，包含目前頁資料、頁碼、每頁筆數、符合條件總筆數與總頁數。
                """)
            .Produces<PagedResult<UserResponse>>(StatusCodes.Status200OK, "application/json")
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        users.MapGet("/summary", GetSummary)
            .WithName("GetUserSummary")
            .WithSummary("用戶數據匯總查詢")
            .WithDescription("""
                統計所在城市、不同性別的用戶總人數。

                此 API 不作分頁，Response 為 JSON 格式，欄位包含：所在城市、性別(男/女)、總人數。
                """)
            .Produces<IReadOnlyList<UserSummaryResponse>>(StatusCodes.Status200OK, "application/json");

        endpoints.MapGet("/api/regions", GetRegions)
            .WithTags("地區可選項")
            .WithName("GetRegions")
            .WithSummary("取得地區可選項")
            .WithDescription("""
                取得規格書附表中的地區可選項。
                """)
            .Produces<IReadOnlyList<RegionOption>>(StatusCodes.Status200OK, "application/json");

        return endpoints;
    }

    private static async Task<IResult> Create(CreateUserRequest request, IUserService userService)
    {
        var result = await userService.CreateAsync(request);
        if (result.EmailAlreadyExists)
        {
            return Results.Conflict(new { message = "電子郵件已存在。" });
        }

        if (!result.IsSuccess)
        {
            var details = new ValidationProblemDetails(
                result.Errors.ToDictionary(error => error, error => new[] { error }));
            return Results.BadRequest(details);
        }

        return Results.CreatedAtRoute("GetUserById", new { id = result.User!.Id }, result.User);
    }

    private static async Task<IResult> GetById(int id, IUserService userService)
    {
        var user = await userService.GetByIdAsync(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    private static async Task<IResult> Search(
        string? name,
        int? minAge,
        int? maxAge,
        string? gender,
        int page,
        IUserService userService)
    {
        if (minAge.HasValue && maxAge.HasValue && minAge > maxAge)
        {
            var details = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["ageRange"] = ["minAge 不可大於 maxAge。"]
            });

            return Results.BadRequest(details);
        }

        if (!TryParseGender(gender, out var parsedGender))
        {
            var details = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["gender"] = ["gender 僅可填入男或女。"]
            });

            return Results.BadRequest(details);
        }

        var result = await userService.SearchAsync(name, minAge, maxAge, parsedGender, page);
        return Results.Ok(result);
    }

    private static bool TryParseGender(string? value, out Gender? gender)
    {
        gender = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        gender = value.Trim() switch
        {
            "男" => Gender.Male,
            "女" => Gender.Female,
            _ => null
        };

        return gender.HasValue;
    }

    private static async Task<IResult> GetSummary(IUserService userService)
    {
        var summary = await userService.GetSummaryAsync();
        return Results.Ok(summary);
    }

    private static IResult GetRegions(IUserService userService)
    {
        var regions = userService.GetRegions();
        return Results.Ok(regions);
    }
}
