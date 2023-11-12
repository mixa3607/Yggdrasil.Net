using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class ValidateRequest
{
    [JsonProperty("clientToken")]
    public string? ClientToken { get; set; }

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; } = null!;
}