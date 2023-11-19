using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;

public class ProfileRequest
{
    [FromQuery(Name = "unsigned")]
    public bool Unsigned { get; set; }

    [FromRoute(Name = "uuid")]
    public Guid UserId { get; set; }
}