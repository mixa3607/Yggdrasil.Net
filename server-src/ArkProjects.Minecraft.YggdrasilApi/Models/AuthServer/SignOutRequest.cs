using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class SignOutRequest
{
    [JsonProperty("username")]
    public string UserName { get; set; } = null!;

    [JsonProperty("password")]
    public string Password { get; set; } = null!;
}