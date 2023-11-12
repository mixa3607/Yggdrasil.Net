using System.Security.Cryptography.X509Certificates;
using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

public static class SharedUser
{
    public static UserProfileModel Profile = new UserProfileModel()
    {
        Id = Guid.NewGuid(),
        Name = "Test1"
    };

    public static UserExtendedProfileModel ExtendedProfile = new UserExtendedProfileModel()
    {
        ProfileId = Profile.Id,
        ProfileName = Profile.Name,
        Timestamp = 0,
        Textures = new Dictionary<string, ProfileTextureModel>()
        {
            {
                ProfileTextureModel.SkinTextureName, new ProfileTextureModel()
                {
                    Url = "https://littleskin.cn/textures/84689d5e10ef00daae0ef4051756f9a4de429cfa1fa2914bb705ee986b1213b3",
                    Metadata = new Dictionary<string, string>()
                    {
                        { "model", "default" }
                    }
                }
            },
            {
                ProfileTextureModel.CapeTextureName, new ProfileTextureModel()
                {
                    Url = "https://littleskin.cn/textures/48173520c0d60d64a85e0162930dfceaa14af390c51eeb5a969a7a9464aa321b",
                    Metadata = new Dictionary<string, string>()
                    {
                        { "model", "default" }
                    }
                }
            }
        }
    };

    public static X509Certificate2 Certificate = new X509Certificate2("files/certs/cert.pfx");

    public static UserModel User = new UserModel()
    {
        Id = Guid.NewGuid(),
        UserName = "Admin1",
        Properties = new[]
        {
            new UserPropertyModel()
            {
                Name = UserPropertyModel.PreferredLangKey,
                Value = "ru-RU"
            }
        }
    };
}