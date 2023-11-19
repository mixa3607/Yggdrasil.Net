using System.Security.Cryptography.X509Certificates;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public class ServerInfo
{
    public required string ServerName { get; set; }

    public required string ImplementationName { get; set; }
    public required string ImplementationVersion { get; set; }

    public required string HomePageUrl { get; set; }
    public required string RegisterUrl { get; set; }

    public required IReadOnlyList<string> SkinDomains { get; set; }

    public required X509Certificate2 Cert { get; set; }
}