using ArkProjects.Minecraft.AspShared.EntityFramework;

namespace ArkProjects.Minecraft.Database.Entities.Users;

public class UserEntity : IEntityWithDeletingFlag
{
    public long Id { get; set; }
    public Guid Guid { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string LoginNormalized { get; set; } = null!;
    public string Login { get; set; } = null!;

    public string EmailNormalized { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public List<string> Permissions { get; set; } = new List<string>();
}