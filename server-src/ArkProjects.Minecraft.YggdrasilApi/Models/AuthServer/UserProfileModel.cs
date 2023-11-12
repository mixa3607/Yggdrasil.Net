using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class UserProfileModel
{
    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("id")]
    public required Guid Id { get; set; }
}