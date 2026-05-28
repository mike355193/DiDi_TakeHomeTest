using DiDi.Enums;

namespace DiDi.Entities;

public sealed class User
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string Name { get; set; }

    public int? Age { get; set; }

    public Gender Gender { get; set; }

    public required string Province { get; set; }

    public required string City { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
