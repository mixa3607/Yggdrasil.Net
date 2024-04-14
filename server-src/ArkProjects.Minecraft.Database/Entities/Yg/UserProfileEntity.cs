using ArkProjects.Minecraft.Database.Entities.Users;

namespace ArkProjects.Minecraft.Database.Entities.Yg;

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