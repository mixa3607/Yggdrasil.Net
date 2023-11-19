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

public class UserAccessTokenEntity
{
    public long Id { get; set; }

    public required string AccessToken { get; set; }
    public required string ClientToken { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset ExpiredAt { get; set; }

    public long ServerId { get; set; }
    public ServerEntity? Server { get; set; }

    public long UserId { get; set; }
    public UserEntity? User { get; set; }
}
public class UserProfileEntity
{
    public long Id { get; set; }

    public required string Name { get; set; }
    public required Guid Guid { get; set; }

    public string? CapeFileUrl { get; set; }
    public string? SkinFileUrl { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

    public long ServerId { get; set; }
    public ServerEntity? Server { get; set; }

    public long UserId { get; set; }
    public UserEntity? User { get; set; }
}
