namespace ArkProjects.Minecraft.Database;

public class ServerEntity
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public required string Name { get; set; }

    public required string HomePageUrl { get; set; }
    public required string RegisterUrl { get; set; }

    public required byte[] PfxCert { get; set; }
}