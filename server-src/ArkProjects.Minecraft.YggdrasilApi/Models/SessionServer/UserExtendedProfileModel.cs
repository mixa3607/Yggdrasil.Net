using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class UserExtendedProfileModel
{
    [JsonProperty("timestamp")]
    public required long Timestamp { get; set; }

    [JsonProperty("profileId")]
    public required Guid ProfileId { get; set; }

    [JsonProperty("profileName")]
    public required string ProfileName { get; set; }

    [JsonProperty("textures")]
    public required IReadOnlyDictionary<string, ProfileTextureModel> Textures { get; set; }
}