using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.User;

public interface IYgUserService
{
    Task<UserEntity?> GetUserByLoginOrEmailAsync(string loginOrEmail, string domain,
        CancellationToken ct = default);

    Task<UserEntity?> GetUserByAccessTokenAsync(string accessToken, string domain,
        CancellationToken ct = default);

    Task<UserProfileEntity?> GetUserProfileByGuidAsync(Guid profileGuid, string domain,
        CancellationToken ct = default);

    Task<string> CreateAccessTokenAsync(string clientToken, Guid userGuid, string domain,
        CancellationToken ct = default);

    Task<bool> ValidateAccessTokenAsync(string? clientToken, string accessToken, string domain,
        CancellationToken ct = default);

    Task<bool> CanRefreshAccessTokenAsync(string? clientToken, string accessToken, string domain,
        CancellationToken ct = default);

    Task InvalidateAccessTokenAsync(Guid userGuid, string accessToken, string domain,
        CancellationToken ct = default);

    Task InvalidateAllAccessTokensAsync(Guid userGuid, string domain,
        CancellationToken ct = default);
}