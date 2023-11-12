using ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("/")]
public class ServerInfoController : ControllerBase
{
    private readonly ILogger<ServerInfoController> _logger;

    public ServerInfoController(ILogger<ServerInfoController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public async Task<ServerInfoModel> GetInfo(CancellationToken ct = default)
    {
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