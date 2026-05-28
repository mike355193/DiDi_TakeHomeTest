using System.Text.RegularExpressions;
using DiDi.Dtos;

namespace DiDi.Validation;

public static partial class UserValidator
{
    private static readonly Dictionary<string, HashSet<string>> Regions = new(StringComparer.Ordinal)
    {
        ["廣東"] = ["廣州", "深圳", "珠海"],
        ["福建"] = ["福州", "廈門"],
        ["廣西"] = ["南寧", "桂林"],
        ["上海"] = ["上海"],
        ["北京"] = ["北京"]
    };

    public static IReadOnlyDictionary<string, HashSet<string>> ValidRegions => Regions;

    public static List<string> ValidateCreateUser(CreateUserRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Email) || !EmailRegex().IsMatch(request.Email))
        {
            errors.Add("Email must be a valid email address.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) ||
            request.Password.Length < 6 ||
            !request.Password.Any(char.IsLetter) ||
            !request.Password.Any(char.IsDigit))
        {
            errors.Add("Password must be at least 6 characters and contain both letters and digits.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add("Name is required.");
        }

        if (request.Age is < 0 or > 130)
        {
            errors.Add("Age must be between 0 and 130.");
        }

        if (!Regions.TryGetValue(request.Province, out var cities) || !cities.Contains(request.City))
        {
            errors.Add("Province and city must match the supported region list.");
        }

        return errors;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
