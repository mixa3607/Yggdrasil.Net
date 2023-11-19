using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using ArkProjects.Minecraft.YggdrasilApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("authserver")]
public class AuthServerController : ControllerBase
{
    private readonly ILogger<AuthServerController> _logger;
    private readonly IYgUserService _userService;

    public AuthServerController(ILogger<AuthServerController> logger, IYgUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("authenticate")]
    public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest req,
        CancellationToken ct = default)
    {
        var user = await _userService.GetUserByLoginOrEmailAsync(req.LoginOrEmail, req.Password, ct);
        var profile = await _userService.GetUserProfileAsync( user.Id, ct);
        var clientToken = req.ClientToken ?? Guid.NewGuid().ToString("N");
        var accessToken = await _userService.CreateAccessTokenAsync(clientToken, user.Id, ct);

        return new AuthenticateResponse()
        {
            ClientToken = clientToken,
            AccessToken = accessToken,
            AvailableProfiles = new[] { profile },
            SelectedProfile = profile,
            User = req.RequestUser ? user : null,
        };
    }

    [HttpPost("refresh")]
    public async Task<RefreshResponse> Refresh([FromBody] RefreshRequest req, CancellationToken ct = default)
    {
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, ct);
        if (!isValid)
        {
            //TODO
        }
        var user = await _userService.GetUserByAccessTokenAsync( req.AccessToken, ct);
        var profile = await _userService.GetUserProfileAsync( user.Id, ct);
        var accessToken = await _userService.CreateAccessTokenAsync(req.ClientToken, user.Id, ct);
        await _userService.InvalidateAccessTokenAsync(user.Id, accessToken, ct);

        return new RefreshResponse()
        {
            SelectedProfile = profile,
            AccessToken = accessToken,
            ClientToken = req.ClientToken,
            User = req.RequestUser ? user : null,
        };
    }

    [HttpPost("validate")]
    public async Task Validate([FromBody] ValidateRequest req, CancellationToken ct = default)
    {
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, ct);
        if (isValid)
        {
            Response.StatusCode = 204;
            return;
        }
        //TODO
    }

    [HttpPost("invalidate")]
    public async Task Invalidate([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, ct);
        if (!isValid)
        {
            //TODO
        }
        var user = await _userService.GetUserByAccessTokenAsync(req.AccessToken, ct);
        await _userService.InvalidateAccessTokenAsync(user.Id, req.AccessToken, ct);
        Response.StatusCode = 204;
    }

    [HttpPost("signout")]
    public async Task SignOut([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        var isValid = await _userService.ValidateAccessTokenAsync(req.ClientToken, req.AccessToken, ct);
        if (!isValid)
        {
            //TODO
        }
        var user = await _userService.GetUserByAccessTokenAsync(req.AccessToken, ct);
        await _userService.InvalidateAllAccessTokensAsync(user.Id, ct);
        Response.StatusCode = 204;
    }
}