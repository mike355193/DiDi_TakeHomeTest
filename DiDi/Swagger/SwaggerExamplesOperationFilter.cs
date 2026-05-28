using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiDi.Swagger;

public sealed class SwaggerExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        switch (operation.OperationId)
        {
            case "CreateUser":
                AddCreateUserExamples(operation);
                break;
            case "GetUserById":
                SetParameterExample(operation, "id", new OpenApiInteger(1), "用戶 ID。");
                AddJsonResponseExample(operation, "200", UserExample(1, "zhang.san@example.com", "張三", 26, "男", "廣東", "廣州"));
                break;
            case "SearchUsers":
                AddSearchUsersExamples(operation);
                break;
            case "GetUserSummary":
                AddJsonResponseExample(operation, "200", Array(
                    Obj(
                        ("city", new OpenApiString("廣州")),
                        ("gender", new OpenApiString("男")),
                        ("totalCount", new OpenApiInteger(20))),
                    Obj(
                        ("city", new OpenApiString("廣州")),
                        ("gender", new OpenApiString("女")),
                        ("totalCount", new OpenApiInteger(5)))));
                break;
            case "GetRegions":
                AddJsonResponseExample(operation, "200", Array(
                    Obj(
                        ("province", new OpenApiString("廣東")),
                        ("cities", Array("廣州", "深圳", "珠海"))),
                    Obj(
                        ("province", new OpenApiString("福建")),
                        ("cities", Array("福州", "廈門")))));
                break;
        }
    }

    private static void AddCreateUserExamples(OpenApiOperation operation)
    {
        AddJsonRequestExample(operation, Obj(
            ("email", new OpenApiString("zhang.san@example.com")),
            ("password", new OpenApiString("abc123")),
            ("name", new OpenApiString("張三")),
            ("age", new OpenApiInteger(26)),
            ("gender", new OpenApiString("男")),
            ("province", new OpenApiString("廣東")),
            ("city", new OpenApiString("廣州"))));

        AddJsonResponseExample(operation, "201", UserExample(1, "zhang.san@example.com", "張三", 26, "男", "廣東", "廣州"));
        AddJsonResponseExample(operation, "409", Obj(("message", new OpenApiString("電子郵件已存在。"))));
    }

    private static void AddSearchUsersExamples(OpenApiOperation operation)
    {
        SetParameterExample(operation, "name", new OpenApiString("張"), "姓名關鍵字，可模糊查詢。");
        SetParameterExample(operation, "minAge", new OpenApiInteger(20), "年齡範圍下限。");
        SetParameterExample(operation, "maxAge", new OpenApiInteger(30), "年齡範圍上限。");
        SetParameterExample(operation, "gender", new OpenApiString("男"), "性別。請填入男或女。");
        SetParameterExample(operation, "page", new OpenApiInteger(1), "頁碼。每頁固定顯示 10 筆。");

        AddJsonResponseExample(operation, "200", Obj(
            ("items", Array(
                UserExample(1, "zhang.san@example.com", "張三", 26, "男", "廣東", "廣州"),
                UserExample(3, "wang.wu@example.com", "王五", 20, "男", "廣東", "深圳"))),
            ("page", new OpenApiInteger(1)),
            ("pageSize", new OpenApiInteger(10)),
            ("totalCount", new OpenApiInteger(2)),
            ("totalPages", new OpenApiInteger(1))));
    }

    private static OpenApiObject UserExample(
        int id,
        string email,
        string name,
        int age,
        string gender,
        string province,
        string city) =>
        Obj(
            ("id", new OpenApiInteger(id)),
            ("email", new OpenApiString(email)),
            ("name", new OpenApiString(name)),
            ("age", new OpenApiInteger(age)),
            ("gender", new OpenApiString(gender)),
            ("province", new OpenApiString(province)),
            ("city", new OpenApiString(city)));

    private static void AddJsonRequestExample(OpenApiOperation operation, IOpenApiAny example)
    {
        if (operation.RequestBody?.Content.TryGetValue("application/json", out var content) == true)
        {
            content.Example = example;
        }
    }

    private static void AddJsonResponseExample(OpenApiOperation operation, string statusCode, IOpenApiAny example)
    {
        if (operation.Responses.TryGetValue(statusCode, out var response) &&
            response.Content.TryGetValue("application/json", out var content))
        {
            content.Example = example;
        }
    }

    private static void SetParameterExample(OpenApiOperation operation, string name, IOpenApiAny example, string description)
    {
        var parameter = operation.Parameters.FirstOrDefault(item => item.Name == name);
        if (parameter is null)
        {
            return;
        }

        parameter.Example = example;
        parameter.Description = description;
    }

    private static OpenApiObject Obj(params (string Name, IOpenApiAny Value)[] values)
    {
        var obj = new OpenApiObject();
        foreach (var (name, value) in values)
        {
            obj[name] = value;
        }

        return obj;
    }

    private static OpenApiArray Array(params IOpenApiAny[] values)
    {
        var array = new OpenApiArray();
        foreach (var value in values)
        {
            array.Add(value);
        }

        return array;
    }

    private static OpenApiArray Array(params string[] values)
    {
        var array = new OpenApiArray();
        foreach (var value in values)
        {
            array.Add(new OpenApiString(value));
        }

        return array;
    }
}
