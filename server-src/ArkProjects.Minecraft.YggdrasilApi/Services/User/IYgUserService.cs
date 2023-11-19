using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

namespace ArkProjects.Minecraft.YggdrasilApi.Services.User;

public interface IYgUserService
{
    Task<UserModel> GetUserByLoginOrEmailAsync(string loginOrEmail, string? password,
        CancellationToken ct = default);

    Task<UserModel> GetUserByAccessTokenAsync(string accessToken,
        CancellationToken ct = default);

    Task<UserProfileModel> GetUserProfileAsync(Guid userGuid,
        CancellationToken ct = default);

    Task<UserExtendedProfileModel> GetUserExtendedProfileAsync(Guid userGuid,
        CancellationToken ct = default);

    Task<string> CreateAccessTokenAsync(string clientToken, Guid userGuid,
        CancellationToken ct = default);

    Task<bool> ValidateAccessTokenAsync(string? clientToken, string accessToken,
        CancellationToken ct = default);

    Task InvalidateAccessTokenAsync(Guid userGuid, string accessToken,
        CancellationToken ct = default);

    Task InvalidateAllAccessTokensAsync(Guid userGuid,
        CancellationToken ct = default);
}