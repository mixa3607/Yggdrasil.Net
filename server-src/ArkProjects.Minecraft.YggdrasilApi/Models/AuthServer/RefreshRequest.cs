using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class RefreshRequest
{
    [JsonProperty("clientToken")]
    public required string ClientToken { get; set; }

    [JsonProperty("accessToken")]
    public required string AccessToken { get; set; }

    [JsonProperty("selectedProfile")]
    public UserProfileModel? SelectedProfile { get; set; }

    [JsonProperty("requestUser")]
    public bool RequestUser { get; set; }
}