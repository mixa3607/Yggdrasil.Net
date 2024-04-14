namespace ArkProjects.Minecraft.Database.Entities.Yg;

public class UserServerJoinEntity
{
    public long Id { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset ExpiredAt { get; set; }
    public required string ServerInstanceId { get; set; }

    public required long UserProfileId { get; set; }
    public UserProfileEntity? UserProfile { get; set; }
}