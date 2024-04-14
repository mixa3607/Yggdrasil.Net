using ArkProjects.Minecraft.AspShared.EntityFramework;

namespace ArkProjects.Minecraft.Database.Entities.Users;

public class UserEntity : IEntityWithDeletingFlag
{
    public long Id { get; set; }
    public required Guid Guid { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public required string LoginNormalized { get; set; }
    public required string Login { get; set; }

    public required string EmailNormalized { get; set; }
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }
}