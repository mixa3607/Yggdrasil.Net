using ArkProjects.Minecraft.Database.Entities;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public interface IYgServerService
{
    Task<ServerEntity?> GetServerInfoAsync(string domain, bool fallbackToDefault, CancellationToken ct = default);
    Task<ServerEntity?> GetServerInfoByProfileAsync(Guid userProfileGuid, CancellationToken ct = default);

    Task JoinProfileToServer(long userProfileId, string serverInstanceId, CancellationToken ct = default);
    Task<Guid?> ProfileJoinedToServer(string userProfileName, string serverInstanceId, CancellationToken ct = default);
}