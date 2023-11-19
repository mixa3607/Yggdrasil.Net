using ArkProjects.Minecraft.YggdrasilApi.Options;
using Microsoft.Extensions.Options;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public class ServerNameProvider : IServerNameProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ServerNodeOptions _options;
    private readonly ILogger<ServerNameProvider> _logger;

    public ServerNameProvider(IHttpContextAccessor httpContextAccessor, IOptions<ServerNodeOptions> options,
        ILogger<ServerNameProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _options = options.Value;
    }

    public string GetServerName()
    {
        var requestDomain = _httpContextAccessor.HttpContext!.Request.Host.Host;
        foreach (var domain in _options.BaseDomains)
        {
            if (requestDomain.EndsWith(domain))
                return requestDomain[..^domain.Length];
        }

        _logger.LogWarning("Request with unknown domain {domain}", requestDomain);
        return "";
    }
}