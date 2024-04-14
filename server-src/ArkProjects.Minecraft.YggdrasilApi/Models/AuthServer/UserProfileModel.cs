using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

public class UserProfileModel
{
    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("id")]
    public required Guid Id { get; set; }

    public static UserProfileModel Map(UserProfileEntity profile)
    {
        return new UserProfileModel()
        {
            Id = profile.Guid,
            Name = profile.Name
        };
    }
}