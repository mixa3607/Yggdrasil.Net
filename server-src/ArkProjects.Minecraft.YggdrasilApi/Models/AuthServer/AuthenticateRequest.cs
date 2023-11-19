using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class AuthenticateRequest
{
    [JsonProperty("agent")]
    public ClientAgentModel? Agent { get; set; }

    [JsonProperty("username")]
    public string LoginOrEmail { get; set; } = null!;

    [JsonProperty("password")]
    public string Password { get; set; } = null!;

    [JsonProperty("clientToken")]
    public string? ClientToken { get; set; }

    [JsonProperty("requestUser")]
    public bool RequestUser { get; set; }
}