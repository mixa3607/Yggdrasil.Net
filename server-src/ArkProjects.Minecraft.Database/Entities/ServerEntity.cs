namespace ArkProjects.Minecraft.Database.Entities;

public class ServerEntity
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// If set then will be used as fallback
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// Yg domain
    /// </summary>
    public string? YgDomain { get; set; }

    public required List<string> SkinDomains { get; set; }
    public List<string>? UploadableTextures { get; set; }


    public required string Name { get; set; }

    public required string HomePageUrl { get; set; }
    public required string RegisterUrl { get; set; }

    public required byte[] PfxCert { get; set; }
}