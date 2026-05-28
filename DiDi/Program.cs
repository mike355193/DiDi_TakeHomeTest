using System.Reflection;
using DiDi.Endpoints;
using DiDi.Entities;
using DiDi.Json;
using DiDi.Repositories;
using DiDi.Services;
using DiDi.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new GenderJsonConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "用戶管理 API",
        Version = "v1",
        Description = "依規格書實作的用戶管理 API。使用 ASP.NET Core Minimal API 開發，提供創建用戶、查找用戶、用戶數據匯總查詢與地區可選項查詢。API 採 RESTful style，request 與 response 皆為 JSON 格式。"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.OperationFilter<SwaggerExamplesOperationFilter>();
});

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:EnsureCreatedOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
    var databaseCreated = db.Database.EnsureCreated();
    if (databaseCreated)
    {
        await UserSeedData.SeedAsync(db);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapUserEndpoints();

app.Run();
