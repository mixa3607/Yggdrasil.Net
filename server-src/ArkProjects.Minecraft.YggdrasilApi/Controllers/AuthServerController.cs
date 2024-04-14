using ArkProjects.Minecraft.YggdrasilApi.Misc;
using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Services.User;
using ArkProjects.Minecraft.YggdrasilApi.Services.UserPassword;
using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("authserver")]
public class AuthServerController : ControllerBase
{
    private readonly ILogger<AuthServerController> _logger;
    private readonly IYgUserService _userService;
    private readonly IUserPasswordService _passwordService;

    public AuthServerController(ILogger<AuthServerController> logger, IYgUserService userService,
        IUserPasswordService passwordService)
    {
        _logger = logger;
        _userService = userService;
        _passwordService = passwordService;
    }

    [HttpPost("authenticate")]
    public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest req,
        CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var user = await _userService.GetUserByLoginOrEmailAsync(req.LoginOrEmail, domain, ct);
        if (user == null)
        {
            throw new YgServerException(ErrorResponseFactory.InvalidCredentials());
        }

        if (!_passwordService.Validate(req.Password, user.PasswordHash))
        {
            throw new YgServerException(ErrorResponseFactory.InvalidCredentials());
        }

        var profile = await _userService.GetUserProfileByGuidAsync(user.Guid, domain, ct);
        if (profile == null)
        {
            throw new YgServerException(ErrorResponseFactory.Custom(400, "PROFILE_NOT_EXIST", "Profile not exist"));
        }

        var profileModel = UserProfileModel.Map(profile);

        var clientToken = req.ClientToken ?? Guid.NewGuid().ToString("N");
        var accessToken = await _userService.CreateAccessTokenAsync(clientToken, user.Guid, domain, ct);

        return new AuthenticateResponse()
        {
            ClientToken = clientToken,
            AccessToken = accessToken,
            AvailableProfiles = new[] { profileModel },
            SelectedProfile = profileModel,
            User = req.RequestUser
                ? new UserModel()
                {
                    Id = user.Guid,
                    UserName = user.Login,
                    Properties = Array.Empty<UserPropertyModel>()
                }
                : null,
        };
    }

    [HttpPost("refresh")]
    public async Task<RefreshResponse> Refresh([FromBody] RefreshRequest req, CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var isValid = await _userService.CanRefreshAccessTokenAsync(req.ClientToken, req.AccessToken, domain, ct);
        if (!isValid)
        {
            throw new YgServerException(ErrorResponseFactory.InvalidToken());
        }

        var user = await _userService.GetUserByAccessTokenAsync(req.AccessToken, domain, ct);
        var profile = await _userService.GetUserProfileByGuidAsync(user!.Guid, domain, ct);
        var accessToken = await _userService.CreateAccessTokenAsync(req.ClientToken, user.Guid, domain, ct);
        await _userService.InvalidateAccessTokenAsync(user.Guid, accessToken, domain, ct);

        return new RefreshResponse()
        {
            SelectedProfile = UserProfileModel.Map(profile!),
            AccessToken = accessToken,
            ClientToken = req.ClientToken,
            User = req.RequestUser ? UserModel.Map(user) : null,
        };
    }

    [HttpPost("validate")]
    public async Task Validate([FromBody] ValidateRequest req, CancellationToken ct = default)
    {
        var domain = HttpContext.Request.Host.Host;
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, domain, ct);
        Response.StatusCode = isValid ? 204 : throw new YgServerException(ErrorResponseFactory.InvalidToken());
    }

    [HttpPost("invalidate")]
    public async Task Invalidate([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    [HttpPost("signout")]
    public async Task SignOut([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        throw new NotImplementedException();
        var domain = HttpContext.Request.Host.Host;
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, domain, ct);
        if (!isValid)
        {
            //TODO
        }

        var user = await _userService.GetUserByAccessTokenAsync(req.AccessToken, domain, ct);
        await _userService.InvalidateAllAccessTokensAsync(user.Guid, domain, ct);
        Response.StatusCode = 204;
    }
}