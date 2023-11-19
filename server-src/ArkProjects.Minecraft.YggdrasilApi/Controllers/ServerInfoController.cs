using ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using SdHub.Services.Tokens;

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
        var info = await _serverService.GetServerInfoAsync("", ct);
        return info;
        return new ServerInfoModel()
        {
            SkinDomains = new string[] { "littleskin.cn" },
            SignaturePublicKey = SharedUser.Certificate.GetRSAPrivateKey()!.ExportSubjectPublicKeyInfoPem(),
            Meta = new ServerMetadataModel()
            {
                ImplementationName = "asp net",
                ImplementationVersion = "1.0.0",
                ServerName = "dev",
                FeatureNonEmailLogin = true,
                Links = new ServerMetadataLinksModel()
                {
                    HomePage = "https://aaaa.com",
                    Register = "https://aaaa.com",
                }
            }
        };
    }
}