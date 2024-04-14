using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.Database.Entities;
using ArkProjects.Minecraft.Database.Entities.Yg;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.Server;

public class YgServerService : IYgServerService
{
    private readonly McDbContext _db;

    public YgServerService(McDbContext db)
    {
        _db = db;
    }

    public async Task<ServerEntity?> GetServerInfoAsync(string domain, bool fallbackToDefault,
        CancellationToken ct = default)
    {
        var server = await _db.Servers
            .Where(x => x.DeletedAt == null && x.YgDomain == domain)
            .SingleOrDefaultAsync(ct);

        if (server == null && fallbackToDefault)
        {
            server = await _db.Servers
                .Where(x => x.DeletedAt == null && x.Default)
                .FirstAsync(ct);
        }

        return server;
    }

    public async Task<ServerEntity?> GetServerInfoByProfileAsync(Guid userProfileGuid, CancellationToken ct = default)
    {
        var server = await _db.UserProfiles
            .Where(x => x.Guid == userProfileGuid)
            .Select(x => x.Server)
            .SingleOrDefaultAsync(ct);
        return server;
    }

    public async Task JoinProfileToServer(long userProfileId, string serverInstanceId, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        _db.UserServerJoins.Add(new UserServerJoinEntity()
        {
            CreatedAt = now,
            ExpiredAt = now.AddMinutes(1),
            ServerInstanceId = serverInstanceId,
            UserProfileId = userProfileId,
        });
        await _db.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<Guid?> ProfileJoinedToServer(string userProfileName, string serverInstanceId, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var j = await _db.UserServerJoins.Where(x =>
                x.UserProfile!.Name == userProfileName &&
                x.ServerInstanceId == serverInstanceId &&
                x.ExpiredAt > now)
            .Select(x=>x.UserProfile!.Guid)
            .FirstOrDefaultAsync(ct);
        return j == default ? null : j;
    }
}