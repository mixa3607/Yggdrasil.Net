using ArkProjects.Minecraft.YggdrasilApi.Models.AuthServer;
using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("authserver")]
public class AuthServerController : ControllerBase
{
    private readonly ILogger<AuthServerController> _logger;

    public AuthServerController(ILogger<AuthServerController> logger)
    {
        _logger = logger;
    }

    [HttpPost("authenticate")]
    public async Task<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest req,
        CancellationToken ct = default)
    {
        var profile = new UserProfileModel()
        {
            Id = Guid.NewGuid(),
            Name = "Test1"
        };
        return new AuthenticateResponse()
        {
            ClientToken = req.ClientToken ?? Guid.NewGuid().ToString("N"),
            AccessToken = "123456789",
            AvailableProfiles = new[] { SharedUser.Profile },
            SelectedProfile = SharedUser.Profile,
            User = req.RequestUser ? SharedUser.User : null,
        };
    }

    [HttpPost("refresh")]
    public async Task<RefreshResponse> Refresh([FromBody] RefreshRequest req, CancellationToken ct = default)
    {
        return new RefreshResponse()
        {
            SelectedProfile = req.SelectedProfile ?? SharedUser.Profile,
            AccessToken = req.AccessToken,
            ClientToken = req.ClientToken ?? Guid.NewGuid().ToString("N"),
            User = req.RequestUser ? SharedUser.User : null,
        };
    }

    [HttpPost("validate")]
    public async Task Validate([FromBody] ValidateRequest req, CancellationToken ct = default)
    {
        Response.StatusCode = 204;
    }

    [HttpPost("invalidate")]
    public async Task Invalidate([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        Response.StatusCode = 204;
    }

    [HttpPost("signout")]
    public async Task SignOut([FromBody] InvalidateRequest req, CancellationToken ct = default)
    {
        Response.StatusCode = 204;
    }
}