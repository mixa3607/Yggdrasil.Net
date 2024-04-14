using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.YggdrasilApi.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("/api/texture")]
public class TextureController : ControllerBase
{
    private readonly McDbContext _db;

    public TextureController(McDbContext db)
    {
        _db = db;
    }

    [HttpGet("{texGuid}")]
    public async Task<ActionResult> GetTexture([FromRoute] Guid texGuid, CancellationToken ct = default)
    {
        var tex = await _db.Textures.FirstOrDefaultAsync(x => x.Guid == texGuid, ct);
        if (tex == null)
        {
            throw new YgServerException(ErrorResponseFactory.Custom(404, "TEXTURE_NOT_FOUND", "Texture not found"));
        }

        return File(tex.File, "image/png");
    }
}