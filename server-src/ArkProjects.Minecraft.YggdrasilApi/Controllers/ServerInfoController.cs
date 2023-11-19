using ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("/")]
public class ServerInfoController : ControllerBase
{
    private readonly ILogger<ServerInfoController> _logger;
    private readonly IYgServerService _serverService;

    public ServerInfoController(ILogger<ServerInfoController> logger, IYgServerService serverService)
    {
        _logger = logger;
        _serverService = serverService;
    }

    [HttpGet()]
    public async Task<ServerInfoModel> GetInfo(CancellationToken ct = default)
    {
        var info = await _serverService.GetServerInfoAsync(ct);
        return new ServerInfoModel()
        {
            SkinDomains = info.SkinDomains,
            SignaturePublicKey = info.Cert.GetRSAPrivateKey()!.ExportSubjectPublicKeyInfoPem(),
            Meta = new ServerMetadataModel()
            {
                ServerName = info.ServerName,
                ImplementationName = info.ImplementationName,
                ImplementationVersion = info.ImplementationVersion,
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