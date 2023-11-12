using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class HasJoinedRequest
{
    [FromQuery(Name = "username")]
    public string UserName { get; set; } = null!;

    [FromQuery(Name = "serverId")]
    public string ServerId { get; set; } = null!;

    [FromQuery(Name = "ip")]
    public string? Ip { get; set; }
}