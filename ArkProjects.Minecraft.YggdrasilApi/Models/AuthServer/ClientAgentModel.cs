using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class ClientAgentModel
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("version")]
    public int? Version { get; set; }
}