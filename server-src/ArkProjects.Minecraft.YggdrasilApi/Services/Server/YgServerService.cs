using System.Security.Cryptography.X509Certificates;
using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.YggdrasilApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public class YgServerService : IYgServerService
{
    private readonly ServerNodeOptions _options;
    private readonly string _serverName;
    private readonly McDbContext _db;

    public YgServerService(IServerNameProvider serverNameProvider, McDbContext db, IOptions<ServerNodeOptions> options)
    {
        _db = db;
        _options = options.Value;
        _serverName = serverNameProvider.GetServerName();
    }

    public async Task<ServerInfo> GetServerInfoAsync(CancellationToken ct = default)
    {
        var server = await _db.Servers
            .Where(x => x.DeletedAt == null && x.Name == _serverName)
            .SingleAsync(ct);

        return new ServerInfo()
        {
            ImplementationName = _options.Implementation,
            ImplementationVersion = _options.Version,
            SkinDomains = _options.SkinDomains,
            Cert = new X509Certificate2(server.PfxCert),
            ServerName = server.Name,
            RegisterUrl = server.RegisterUrl,
            HomePageUrl = server.HomePageUrl,
        };
    }
}