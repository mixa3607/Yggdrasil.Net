using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.User;

public class YgUserService : IYgUserService
{
    private readonly McDbContext _db;

    public YgUserService(McDbContext db)
    {
        _db = db;
    }

    public async Task<UserEntity?> GetUserByLoginOrEmailAsync(string loginOrEmail, string domain,
        CancellationToken ct = default)
    {
        var n = loginOrEmail.Normalize().ToUpper();
        var user = await _db.Users
            .AsNoTracking()
            .Where(x => (x.EmailNormalized == n || x.LoginNormalized == n) && x.DeletedAt == null)
            .FirstOrDefaultAsync(ct);
        return user;
    }

    public async Task<UserEntity?> GetUserByAccessTokenAsync(string accessToken, string domain,
        CancellationToken ct = default)
    {
        var user = await _db.UserAccessTokens
            .AsNoTracking()
            .Where(x =>
                x.AccessToken == accessToken &&
                x.Server!.YgDomain == domain &&
                x.User!.DeletedAt == null)
            .Select(x => x.User)
            .FirstOrDefaultAsync(ct);
        return user;
    }

    public Task<UserProfileEntity?> GetUserProfileByGuidAsync(Guid profileGuid, string domain, CancellationToken ct = default)
    {
        return GetUserExtendedProfileAsync(profileGuid, null, domain, ct);
    }

    private async Task<UserProfileEntity?> GetUserExtendedProfileAsync(Guid? profileGuid, string? profileName,
        string domain,
        CancellationToken ct = default)
    {
        var query = _db.UserProfiles
            .AsNoTracking()
            .Where(x =>
                x.Server!.YgDomain == domain &&
                x.User!.DeletedAt == null);
        if (profileGuid != null)
        {
            query = query.Where(x => x.Guid == profileGuid.Value);
        }
        else if (!string.IsNullOrWhiteSpace(profileName))
        {
            query = query.Where(x => x.Name == profileName);
        }
        else
        {
            throw new ArgumentNullException("", "guid or name must be not null");
        }

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<string> CreateAccessTokenAsync(string clientToken, Guid userGuid, string domain,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var user = await _db.Users
            .Where(x => x.DeletedAt == null && x.Guid == userGuid)
            .FirstAsync(ct);
        var server = await _db.Servers
            .Where(x => x.DeletedAt == null && x.YgDomain == domain)
            .FirstAsync(ct);

        var exp = TimeSpan.FromDays(2);
        var refr = TimeSpan.FromDays(28);
        var entity = new UserAccessTokenEntity()
        {
            Server = server,
            User = user,
            AccessToken = Guid.NewGuid().ToString(),
            ClientToken = clientToken,
            ExpiredAt = now + exp,
            MustBeRefreshedAt = now + refr,
            CreatedAt = now,
        };
        _db.UserAccessTokens.Add(entity);
        await _db.SaveChangesAsync(CancellationToken.None);
        return entity.AccessToken;
    }

    public async Task<bool> ValidateAccessTokenAsync(string? clientToken, string accessToken, string domain,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var isValid = await _db.UserAccessTokens
            .Where(x =>
                (x.ClientToken == clientToken || clientToken == null) &&
                x.User!.DeletedAt == null &&
                x.AccessToken == accessToken &&
                x.ExpiredAt > now &&
                x.Server!.YgDomain == domain)
            .AnyAsync(ct);
        return isValid;
    }

    public async Task<bool> CanRefreshAccessTokenAsync(string? clientToken, string accessToken, string domain,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var isValid = await _db.UserAccessTokens
            .Where(x =>
                (x.ClientToken == clientToken || clientToken == null) &&
                x.User!.DeletedAt == null &&
                x.AccessToken == accessToken &&
                x.MustBeRefreshedAt > now &&
                x.Server!.YgDomain == domain)
            .AnyAsync(ct);
        return isValid;
    }

    public async Task InvalidateAccessTokenAsync(Guid userGuid, string accessToken, string domain,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var tokenEntity = await _db.UserAccessTokens
            .Where(x =>
                x.User!.Guid == userGuid &&
                x.User!.DeletedAt == null &&
                x.AccessToken == accessToken &&
                x.ExpiredAt > now &&
                x.Server!.YgDomain == domain)
            .FirstAsync(ct);
        tokenEntity.ExpiredAt = now;
        tokenEntity.MustBeRefreshedAt = now;
        await _db.SaveChangesAsync(CancellationToken.None);
    }

    public async Task InvalidateAllAccessTokensAsync(Guid userGuid, string domain, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var tokenEntities = await _db.UserAccessTokens
            .Where(x =>
                x.User!.Guid == userGuid &&
                x.User!.DeletedAt == null &&
                x.ExpiredAt > now &&
                x.Server!.YgDomain == domain)
            .ToArrayAsync(ct);
        foreach (var tokenEntity in tokenEntities)
        {
            tokenEntity.ExpiredAt = now;
            tokenEntity.MustBeRefreshedAt = now;
        }

        await _db.SaveChangesAsync(CancellationToken.None);
    }
}