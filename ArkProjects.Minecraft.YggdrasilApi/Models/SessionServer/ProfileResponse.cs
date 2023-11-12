using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class ProfileResponse
{
    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("id")]
    public required Guid Id { get; set; }

    /// <summary>
    /// <see cref="KnownProfileActions"/>
    /// </summary>
    [JsonProperty("profileActions")]
    public required IReadOnlyList<string> ProfileActions { get; set; }

    [JsonProperty("properties")]
    public required IReadOnlyList<PropertyModel> Properties { get; set; }

    /// <param name="Name"><see cref="KnownProfileProperties"/></param>
    /// <param name="Value"></param>
    /// <param name="Signature"></param>
    public record PropertyModel(string Name, string Value, string? Signature);
}