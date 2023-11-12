using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class AuthenticateResponse
{
    [JsonProperty("user")]
    public UserModel? User { get; set; }

    [JsonProperty("clientToken")]
    public required string ClientToken { get; set; }

    [JsonProperty("accessToken")]
    public required string AccessToken { get; set; }

    [JsonProperty("availableProfiles")]
    public required IReadOnlyList<UserProfileModel> AvailableProfiles { get; set; }

    [JsonProperty("selectedProfile")]
    public UserProfileModel? SelectedProfile { get; set; }
}