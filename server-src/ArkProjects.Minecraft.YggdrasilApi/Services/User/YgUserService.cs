using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
using ArkProjects.Minecraft.YggdrasilApi.Services.UserPassword;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.User;

public class YgUserService : IYgUserService
{
    private readonly IUserPasswordService _passwordService;
    private readonly string _serverName;
    private readonly McDbContext _db;

    public YgUserService(IUserPasswordService passwordService, IServerNameProvider serverNameProvider, McDbContext db)
    {
        _passwordService = passwordService;
        _serverName = serverNameProvider.GetServerName();
        _db = db;
    }

    public async Task<UserModel> GetUserByLoginOrEmailAsync(string loginOrEmail, string? password,
        CancellationToken ct = default)
    {
        var n = loginOrEmail.ToUpper().Normalize();
        var passHash = password == null ? null : _passwordService.CreatePasswordHash(password);
        var user = await _db.Users
            .Where(x =>
                (x.EmailNormalized == n || x.LoginNormalized == n) &&
                (passHash == null || x.PasswordHash == passHash) &&
                x.DeletedAt == null
            )
            .FirstOrDefaultAsync(ct);

        return new UserModel()
        {
            Id = user.Guid,
            UserName = user.Login,
            Properties = Array.Empty<UserPropertyModel>()
        };
    }

    public async Task<UserModel> GetUserByAccessTokenAsync(string accessToken, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var user = await _db.UserAccessTokens
            .Where(x => x.ExpiredAt > now &&
                        x.AccessToken == accessToken &&
                        x.Server!.Name == _serverName &&
                        x.User!.DeletedAt == null)
            .Select(x => x.User)
            .FirstOrDefaultAsync(ct);

        return new UserModel()
        {
            Id = user.Guid,
            UserName = user.Login,
            Properties = Array.Empty<UserPropertyModel>()
        };
    }

    public async Task<UserProfileModel> GetUserProfileAsync(Guid userGuid, CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .Where(x => x.Server!.Name == _serverName &&
                        x.User!.DeletedAt == null &&
                        x.User.Guid == userGuid)
            .FirstOrDefaultAsync(ct);
        return new UserProfileModel()
        {
            Id = profile.Guid,
            Name = profile.Name
        };
    }

    public async Task<UserExtendedProfileModel> GetUserExtendedProfileAsync(Guid userGuid,
        CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .Where(x => x.Server!.Name == _serverName &&
                        x.User!.DeletedAt == null &&
                        x.User.Guid == userGuid)
            .FirstOrDefaultAsync(ct);

        var textures = new Dictionary<string, ProfileTextureModel>();
        if (profile.CapeFileUrl != null)
        {
            textures[ProfileTextureModel.CapeTextureName] = new ProfileTextureModel()
            {
                Url = profile.CapeFileUrl,
                Metadata = new Dictionary<string, string>(0)
            };
        }

        if (profile.SkinFileUrl != null)
        {
            textures[ProfileTextureModel.SkinTextureName] = new ProfileTextureModel()
            {
                Url = profile.SkinFileUrl,
                Metadata = new Dictionary<string, string>(0)
            };
        }

        return new UserExtendedProfileModel()
        {
            ProfileId = profile.Guid,
            ProfileName = profile.Name,
            Textures = textures,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }

    public async Task<string> CreateAccessTokenAsync(string clientToken, Guid userGuid, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var user = await _db.Users
            .Where(x => x.DeletedAt == null && x.Guid == userGuid)
            .FirstOrDefaultAsync(ct);
        var server = await _db.Servers
            .Where(x => x.DeletedAt == null && x.Name == _serverName)
            .FirstOrDefaultAsync(ct);
        var entity = new UserAccessTokenEntity()
        {
            Server = server,
            User = user,
            AccessToken = Guid.NewGuid().ToString(),
            ClientToken = clientToken,
            ExpiredAt = now.AddMonths(1),
            CreatedAt = now,
        };
        _db.UserAccessTokens.Add(entity);
        await _db.SaveChangesAsync(CancellationToken.None);
        return entity.AccessToken;
    }

    public async Task<bool> ValidateAccessTokenAsync(string? clientToken, string accessToken,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var isValid = await _db.UserAccessTokens
            .Where(x =>
                (x.ClientToken == clientToken || clientToken == null) &&
                x.User!.DeletedAt == null &&
                x.AccessToken == accessToken &&
                x.ExpiredAt > now &&
                x.Server!.Name == _serverName)
            .AnyAsync(ct);
        return isValid;
    }

    public async Task InvalidateAccessTokenAsync(Guid userGuid, string accessToken,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var tokenEntity = await _db.UserAccessTokens
            .Where(x =>
                x.User!.Guid == userGuid &&
                x.User!.DeletedAt == null &&
                x.AccessToken == accessToken &&
                x.ExpiredAt > now &&
                x.Server!.Name == _serverName)
            .FirstOrDefaultAsync(ct);
        tokenEntity.ExpiredAt = now;
        await _db.SaveChangesAsync(CancellationToken.None);
    }

    public async Task InvalidateAllAccessTokensAsync(Guid userGuid, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var tokenEntities = await _db.UserAccessTokens
            .Where(x =>
                x.User!.Guid == userGuid &&
                x.User!.DeletedAt == null &&
                x.ExpiredAt > now &&
                x.Server!.Name == _serverName)
            .ToArrayAsync(ct);
        foreach (var tokenEntity in tokenEntities)
        {
            tokenEntity.ExpiredAt = now;
        }

        await _db.SaveChangesAsync(CancellationToken.None);
    }
}