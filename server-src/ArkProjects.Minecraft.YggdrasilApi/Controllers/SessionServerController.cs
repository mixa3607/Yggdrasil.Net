using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ArkProjects.Minecraft.YggdrasilApi.Misc.JsonConverters;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("sessionserver")]
public class SessionServerController : ControllerBase
{
    private readonly ILogger<SessionServerController> _logger;

    public SessionServerController(ILogger<SessionServerController> logger)
    {
        _logger = logger;
    }

    [HttpPost("session/minecraft/join")]
    public async Task Join([FromBody] JoinRequest req, CancellationToken ct = default)
    {
        Response.StatusCode = 204;
    }

    [HttpPost("session/minecraft/hasJoined")]
    public async Task HasJoined([FromQuery] HasJoinedRequest req, CancellationToken ct = default)
    {
        Response.StatusCode = 204;
    }

    [HttpGet("session/minecraft/profile/{uuid}")]
    public async Task<ProfileResponse> Profile([FromQuery] ProfileRequest req, CancellationToken ct = default)
    {
        SharedUser.ExtendedProfile.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var jStr = JsonConvert.SerializeObject(SharedUser.ExtendedProfile, new YggdrasilGuidConverter());
        var b64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(jStr));

        return new ProfileResponse
        {
            Id = SharedUser.ExtendedProfile.ProfileId,
            Name = SharedUser.ExtendedProfile.ProfileName,
            ProfileActions = new string[] { KnownProfileActions.ForcedNameChange, KnownProfileActions.UsingBannedSkin },
            Properties = new ProfileResponse.PropertyModel[]
            {
                new(KnownProfileProperties.Textures, b64Str, req.Unsigned ? null : GetSign(b64Str))
            }
        };
    }

    public static string GetSign(string text)
    {
        var sign = SharedUser.Certificate.GetRSAPrivateKey()!
            .SignData(Encoding.UTF8.GetBytes(text), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(sign);
    }
}