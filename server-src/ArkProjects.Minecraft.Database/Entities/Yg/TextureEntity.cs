namespace ArkProjects.Minecraft.Database.Entities.Yg;

public class TextureEntity
{
    public long Id { get; set; }
    public required Guid Guid { get; set; }
    public required string Texture { get; set; }
    public required byte[] File { get; set; }
    public required byte[] Sha256 { get; set; }
}