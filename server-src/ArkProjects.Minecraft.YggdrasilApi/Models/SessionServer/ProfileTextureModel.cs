using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class ProfileTextureModel
{
    public const string SkinTextureName = "SKIN";
    public const string CapeTextureName = "CAPE";

    [JsonProperty("url")]
    public required string Url { get; set; }

    [JsonProperty("metadata")]
    public required Dictionary<string, string> Metadata { get; set; }
}