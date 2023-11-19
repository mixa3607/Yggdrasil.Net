using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;
using ArkProjects.Minecraft.YggdrasilApi.Services.User;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("sessionserver")]
public class SessionServerController : ControllerBase
{
    private readonly ILogger<SessionServerController> _logger;
    private readonly IYgUserService _userService;
    private readonly IJsonHelper _jsonHelper;

    public SessionServerController(ILogger<SessionServerController> logger, IYgUserService userService,
        IJsonHelper jsonHelper)
    {
        _logger = logger;
        _userService = userService;
        _jsonHelper = jsonHelper;
    }

    [HttpPost("session/minecraft/join")]
    public async Task Join([FromBody] JoinRequest req, CancellationToken ct = default)
    {
        //ignore
        Response.StatusCode = 204;
    }

    [HttpPost("session/minecraft/hasJoined")]
    public async Task HasJoined([FromQuery] HasJoinedRequest req, CancellationToken ct = default)
    {
        var user = await _userService.GetUserByLoginOrEmailAsync(req.UserName, null, ct);
        Response.StatusCode = 204;
    }

    [HttpGet("session/minecraft/profile/{uuid}")]
    public async Task<ProfileResponse> Profile(ProfileRequest req, CancellationToken ct = default)
    {
        var extProfile = await _userService.GetUserExtendedProfileAsync(req.UserId, ct);
        var extProfileJson = _jsonHelper.Serialize(extProfile) as HtmlString;
        var extProfileBytes = Encoding.UTF8.GetBytes(extProfileJson!.Value!);
        var extProfileB64 = Convert.ToBase64String(extProfileBytes);

        return new ProfileResponse
        {
            Id = extProfile.ProfileId,
            Name = extProfile.ProfileName,
            ProfileActions = Array.Empty<string>(),
            Properties = new ProfileResponse.PropertyModel[]
            {
                new(KnownProfileProperties.Textures, extProfileB64, req.Unsigned ? null : GetSign(extProfileB64))
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