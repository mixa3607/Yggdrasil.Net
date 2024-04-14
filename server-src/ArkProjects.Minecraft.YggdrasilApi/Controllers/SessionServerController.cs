using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ArkProjects.Minecraft.YggdrasilApi.Misc;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
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
    private readonly IYgServerService _serverService;
    private readonly IJsonHelper _jsonHelper;

    public SessionServerController(ILogger<SessionServerController> logger, IYgUserService userService,
        IJsonHelper jsonHelper, IYgServerService serverService)
    {
        _logger = logger;
        _userService = userService;
        _jsonHelper = jsonHelper;
        _serverService = serverService;
    }

    [HttpPost("session/minecraft/join")]
    public async Task Join([FromBody] JoinRequest req, CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;

        var isValid = await _userService.ValidateAccessTokenAsync(null, req.AccessToken, domain, ct);
        if (!isValid)
        {
            throw new YgServerException(ErrorResponseFactory.InvalidToken());
        }

        var profile = await _userService.GetUserProfileByGuidAsync(req.SelectedProfile, domain, ct);
        await _serverService.JoinProfileToServer(profile!.Id, req.ServerId, ct);
    }

    [HttpGet("session/minecraft/hasJoined")]
    public async Task<HasJoinedResponse> HasJoined([FromQuery] HasJoinedRequest req, CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var profileGuid = await _serverService.ProfileJoinedToServer(req.UserName, req.ServerId, ct);
        if (profileGuid == null)
        {
            throw new YgServerException(ErrorResponseFactory.Custom(400, "PROFILE_NOT_JOINED", "Profile not joined"));
        }

        var profile = await _userService.GetUserProfileByGuidAsync(profileGuid.Value, domain, ct);
        var extProfile = UserExtendedProfileModel.Map(profile!);
        var extProfileJson = _jsonHelper.Serialize(extProfile) as HtmlString;
        var extProfileBytes = Encoding.UTF8.GetBytes(extProfileJson!.Value!);
        var extProfileB64 = Convert.ToBase64String(extProfileBytes);
        var server = await _serverService.GetServerInfoByProfileAsync(extProfile!.ProfileId, ct);
        
        return new HasJoinedResponse()
        {
            Id = extProfile.ProfileId,
            Name = extProfile.ProfileName,
            Properties = new HasJoinedResponse.PropertyModel[]
            {
                new(KnownProfileProperties.Textures, extProfileB64, GetSign(new X509Certificate2(server!.PfxCert), extProfileB64)),
            }
        };
    }

    [HttpGet("session/minecraft/profile/{uuid}")]
    public async Task<ProfileResponse> Profile(ProfileRequest req, CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var profile = await _userService.GetUserProfileByGuidAsync(req.UserId, domain, ct);
        if (profile == null)
        {
            throw new YgServerException(ErrorResponseFactory.Custom(400, "PROFILE_NOT_EXIST", "Profile not exist"));
        }

        var extProfile = UserExtendedProfileModel.Map(profile);
        var extProfileJson = _jsonHelper.Serialize(extProfile) as HtmlString;
        var extProfileBytes = Encoding.UTF8.GetBytes(extProfileJson!.Value!);
        var extProfileB64 = Convert.ToBase64String(extProfileBytes);

        var server = await _serverService.GetServerInfoByProfileAsync(profile.Guid, ct);

        return new ProfileResponse
        {
            Id = profile.Guid,
            Name = profile.Name,
            ProfileActions = Array.Empty<string>(),
            Properties = new ProfileResponse.PropertyModel[]
            {
                new(KnownProfileProperties.Textures, extProfileB64, req.Unsigned
                    ? null
                    : GetSign(new X509Certificate2(server!.PfxCert), extProfileB64)),
                new(KnownProfileProperties.UploadableTextures,
                    string.Join(',', server!.UploadableTextures ?? new List<string>()), null)
            }
        };
    }

    private static string GetSign(X509Certificate2 cert, string text)
    {
        var sign = cert
            .GetRSAPrivateKey()!
            .SignData(Encoding.UTF8.GetBytes(text), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(sign);
    }
}