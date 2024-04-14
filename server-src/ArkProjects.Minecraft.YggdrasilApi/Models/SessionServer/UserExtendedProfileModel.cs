using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
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

    public static UserExtendedProfileModel Map(UserProfileEntity profile)
    {
        var textures = new Dictionary<string, ProfileTextureModel>();
        if (profile.CapeFileUrl != null)
        {
            textures[ProfileTextureModel.CapeTextureName] = new ProfileTextureModel()
            {
                Url = profile.CapeFileUrl,
                Metadata = new Dictionary<string, string>(0)
            };
        }

        if (profile.SkinFileUrl != null)
        {
            textures[ProfileTextureModel.SkinTextureName] = new ProfileTextureModel()
            {
                Url = profile.SkinFileUrl,
                Metadata = new Dictionary<string, string>(0)
            };
        }

        return new UserExtendedProfileModel()
        {
            ProfileId = profile.Guid,
            ProfileName = profile.Name,
            Textures = textures,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }
}