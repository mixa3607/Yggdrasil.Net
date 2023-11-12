using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;

public class ServerInfoModel
{
    [JsonProperty("meta")]
    public ServerMetadataModel? Meta { get; set; }

    [JsonProperty("skinDomains")]
    public IReadOnlyList<string> SkinDomains { get; set; }

    [JsonProperty("signaturePublickey")]
    public string? SignaturePublicKey { get; set; }
}