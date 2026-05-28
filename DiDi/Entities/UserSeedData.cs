using DiDi.Enums;

namespace DiDi.Entities;

public static class UserSeedData
{
    private const string DefaultPasswordHash = "E7CF3EF4F17C3999A94F2C6F612E8A888E5B1026878E4E19398B23BD38EC221A";

    public static async Task SeedAsync(UserManagementDbContext db)
    {
        db.Users.AddRange(
            new User { Email = "zhang.san@example.com", PasswordHash = DefaultPasswordHash, Name = "張三", Age = 26, Gender = Gender.Male, Province = "廣東", City = "廣州" },
            new User { Email = "li.si@example.com", PasswordHash = DefaultPasswordHash, Name = "李四", Age = 25, Gender = Gender.Female, Province = "上海", City = "上海" },
            new User { Email = "wang.wu@example.com", PasswordHash = DefaultPasswordHash, Name = "王五", Age = 20, Gender = Gender.Male, Province = "廣東", City = "深圳" },
            new User { Email = "zhao.liu@example.com", PasswordHash = DefaultPasswordHash, Name = "趙六", Age = 31, Gender = Gender.Female, Province = "廣東", City = "廣州" },
            new User { Email = "chen.qi@example.com", PasswordHash = DefaultPasswordHash, Name = "陳七", Age = 29, Gender = Gender.Male, Province = "廣東", City = "珠海" },
            new User { Email = "lin.ba@example.com", PasswordHash = DefaultPasswordHash, Name = "林八", Age = 34, Gender = Gender.Female, Province = "福建", City = "福州" },
            new User { Email = "huang.jiu@example.com", PasswordHash = DefaultPasswordHash, Name = "黃九", Age = 22, Gender = Gender.Male, Province = "福建", City = "廈門" },
            new User { Email = "wu.shi@example.com", PasswordHash = DefaultPasswordHash, Name = "吳十", Age = 27, Gender = Gender.Female, Province = "廣西", City = "南寧" },
            new User { Email = "zhou.yi@example.com", PasswordHash = DefaultPasswordHash, Name = "周一", Age = 24, Gender = Gender.Male, Province = "廣西", City = "桂林" },
            new User { Email = "xu.er@example.com", PasswordHash = DefaultPasswordHash, Name = "徐二", Age = 30, Gender = Gender.Female, Province = "北京", City = "北京" },
            new User { Email = "sun.san@example.com", PasswordHash = DefaultPasswordHash, Name = "孫三", Age = 28, Gender = Gender.Male, Province = "北京", City = "北京" },
            new User { Email = "gao.si@example.com", PasswordHash = DefaultPasswordHash, Name = "高四", Age = 23, Gender = Gender.Female, Province = "廣東", City = "深圳" });

        await db.SaveChangesAsync();
    }
}
