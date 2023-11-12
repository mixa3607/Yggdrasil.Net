using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class RefreshRequest
{
    [JsonProperty("clientToken")]
    public string? ClientToken { get; set; }

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("selectedProfile")]
    public UserProfileModel? SelectedProfile { get; set; }

    [JsonProperty("requestUser")]
    public bool RequestUser { get; set; }
}