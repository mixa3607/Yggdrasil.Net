using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class UserPropertyModel
{
    public const string PreferredLangKey = "preferredLanguage";

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("value")]
    public required string Value { get; set; }
}