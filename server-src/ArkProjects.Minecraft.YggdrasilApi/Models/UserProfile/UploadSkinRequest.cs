using Microsoft.AspNetCore.Mvc;

namespace ArkProjects.Minecraft.YggdrasilApi.Models.UserProfile;

public class UploadSkinRequest
{
    [FromForm(Name = "file")]
    public required IFormFile File { get; set; }

    [FromForm(Name = "model")]
    public string? Model { get; set; }

    [FromRoute(Name = "uuid")]
    public Guid UserId { get; set; }
}