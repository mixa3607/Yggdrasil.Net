using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class JoinRequest
{
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("selectedProfile")]
    public Guid SelectedProfile { get; set; }

    [JsonProperty("serverId")]
    public string ServerId { get; set; } = null!;
}