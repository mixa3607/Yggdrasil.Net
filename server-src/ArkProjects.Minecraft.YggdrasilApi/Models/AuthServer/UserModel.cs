using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class UserModel
{
    [JsonProperty("username")]
    public required string UserName { get; set; }

    [JsonProperty("id")]
    public required Guid Id { get; set; }

    [JsonProperty("properties")]
    public required IReadOnlyList<UserPropertyModel> Properties { get; set; }
}