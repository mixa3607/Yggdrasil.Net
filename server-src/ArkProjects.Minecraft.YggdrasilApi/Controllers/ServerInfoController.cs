using ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
using ArkProjects.Minecraft.YggdrasilApi.Options;
using Microsoft.Extensions.Options;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("/")]
public class ServerInfoController : ControllerBase
{
    private readonly ILogger<ServerInfoController> _logger;
    private readonly IYgServerService _serverService;
    private readonly ServerNodeOptions _options;

    public ServerInfoController(ILogger<ServerInfoController> logger, IYgServerService serverService, IOptions<ServerNodeOptions> options)
    {
        _logger = logger;
        _serverService = serverService;
        _options = options.Value;
    }

    [HttpGet()]
    public async Task<ServerInfoModel> GetInfo(CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var info = await _serverService.GetServerInfoAsync(domain, true, ct);
        return new ServerInfoModel()
        {
            SkinDomains = info!.SkinDomains,
            SignaturePublicKey = new X509Certificate2(info.PfxCert).GetRSAPrivateKey()!.ExportSubjectPublicKeyInfoPem(),
            Meta = new ServerMetadataModel()
            {
                ServerName = info.Name,
                ImplementationName = _options.Implementation,
                ImplementationVersion = _options.Version,
                FeatureNonEmailLogin = true,
                Links = new ServerMetadataLinksModel()
                {
                    HomePage = info.HomePageUrl,
                    Register = info.RegisterUrl,
                }
            }
        };
    }
}