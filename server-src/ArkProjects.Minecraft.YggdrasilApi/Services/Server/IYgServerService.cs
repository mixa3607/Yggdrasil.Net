namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public interface IYgServerService
{
    Task<ServerInfo> GetServerInfoAsync(CancellationToken ct = default);
}