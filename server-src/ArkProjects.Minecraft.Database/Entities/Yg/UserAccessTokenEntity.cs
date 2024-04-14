using ArkProjects.Minecraft.Database.Entities.Users;

namespace ArkProjects.Minecraft.Database.Entities.Yg;

public class UserAccessTokenEntity
{
    public long Id { get; set; }

    public required string AccessToken { get; set; }
    public required string ClientToken { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset ExpiredAt { get; set; }
    public required DateTimeOffset MustBeRefreshedAt { get; set; }

    public long ServerId { get; set; }
    public ServerEntity? Server { get; set; }

    public long UserId { get; set; }
    public UserEntity? User { get; set; }
}