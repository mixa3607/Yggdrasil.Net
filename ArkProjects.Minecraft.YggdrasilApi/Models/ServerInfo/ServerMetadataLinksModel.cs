using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;

public class ServerMetadataLinksModel
{
    [JsonProperty("homepage")]
    public required string HomePage { get; set; }

    [JsonProperty("register")]
    public required string Register { get; set; }
}