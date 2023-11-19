using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class InvalidateRequest
{
    [JsonProperty("clientToken")]
    public required string ClientToken { get; set; }

    [JsonProperty("accessToken")]
    public required string AccessToken { get; set; }
}