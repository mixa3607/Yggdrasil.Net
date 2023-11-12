using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;

public class ServerMetadataModel
{
    [JsonProperty("implementationName")]
    public required string ImplementationName { get; set; }

    [JsonProperty("implementationVersion")]
    public required string ImplementationVersion { get; set; }

    [JsonProperty("serverName")]
    public required string ServerName { get; set; }

    [JsonProperty("feature.non_email_login")]
    public bool FeatureNonEmailLogin { get; set; }

    [JsonProperty("links")]
    public ServerMetadataLinksModel Links { get; set; }
}