using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Models.ServerInfo;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

namespace SdHub.Services.Tokens;

public interface IUserPasswordService
{
    bool CheckPasswordRequirements(string? password);
    string CreatePasswordHash(string password);
    bool Validate(string? password, string? passwordHash);
}

public interface IYgServerService
{
    Task<ServerInfoModel> GetServerInfoAsync(string name,
        CancellationToken ct = default);
}

public interface IYgUserService
{
    Task<UserModel> GetUserByLoginOrEmailAsync(string loginOrEmail, string password,
        CancellationToken ct = default);

    Task<UserModel> GetUserByAccessTokenAsync(string accessToken,
        CancellationToken ct = default);

    Task<UserProfileModel> GetUserProfileAsync(string serverName, Guid userGuid,
        CancellationToken ct = default);

    Task<UserExtendedProfileModel> GetUserExtendedProfileAsync(string serverName, Guid userGuid,
        CancellationToken ct = default);

    Task<string> CreateAccessTokenAsync(string clientToken, Guid userGuid,
        CancellationToken ct = default);

    Task<bool> ValidateAccessTokenAsync(string clientToken, string accessToken,
        CancellationToken ct = default);

    Task<bool> InvalidateAccessTokenAsync(Guid userGuid, string accessToken,
        CancellationToken ct = default);

    Task<bool> InvalidateAllAccessTokensAsync(Guid userGuid,
        CancellationToken ct = default);
}