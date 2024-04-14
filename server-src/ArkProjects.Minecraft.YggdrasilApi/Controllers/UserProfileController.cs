using System.Security.Cryptography;
using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
using ArkProjects.Minecraft.YggdrasilApi.Misc;
using ArkProjects.Minecraft.YggdrasilApi.Models.SessionServer;
using ArkProjects.Minecraft.YggdrasilApi.Models.UserProfile;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
using ArkProjects.Minecraft.YggdrasilApi.Services.User;
using MetadataExtractor.Formats.Png;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Controllers;

[ApiController]
[Route("/api/user/profile")]
public class UserProfileController : ControllerBase
{
    private readonly ILogger<UserProfileController> _logger;
    private readonly IYgServerService _serverService;
    private readonly IYgUserService _userService;
    private readonly McDbContext _db;

    public UserProfileController(ILogger<UserProfileController> logger, IYgServerService serverService,
        IYgUserService userService, McDbContext db)
    {
        _logger = logger;
        _serverService = serverService;
        _userService = userService;
        _db = db;
    }

    //TODO rewrite
    [HttpPut("{uuid}/skin")]
    public async Task<ActionResult> UploadSkin(UploadSkinRequest req, CancellationToken ct = default)
    {
        var isSlim = req.Model == "slim";
        var domain = HttpContext.Request.Host.Host;
        var accessToken = GetAccessToken();
        if (accessToken == null)
        {
            throw new YgServerException(ErrorResponseFactory.InvalidToken());
        }

        var isValid = await _userService.ValidateAccessTokenAsync(null, accessToken, domain, ct);
        if (!isValid)
        {
            throw new YgServerException(ErrorResponseFactory.InvalidToken());
        }

        if (req.File.Length > 1024 * 300)
        {
            throw new YgServerException(ErrorResponseFactory.Custom(400, "FILE_SIZE_LIMIT", "Max file size is 300Kb"));
        }


        await ApplyTextureAsync(req, ct);

        return Ok();
    }

    private async Task ApplyTextureAsync(UploadSkinRequest req, CancellationToken ct = default)
    {
        var server = await _serverService.GetServerInfoByProfileAsync(req.UserId, ct);

        var imageBytes = ReadFile(req.File);
        var dims = ReadDimensions(imageBytes);
        var sha256 = SHA256.HashData(imageBytes);
        var texture = dims switch
        {
            (64, 64) => ProfileTextureModel.SkinTextureName,
            (32, 64) => ProfileTextureModel.CapeTextureName,
            _ => throw new NotSupportedException("File is not supported"),
        };
        var textureEntity = new TextureEntity()
        {
            File = imageBytes,
            Guid = Guid.NewGuid(),
            Sha256 = sha256,
            Texture = texture,
        };
        _db.Textures.Add(textureEntity);
        var url = $"https://{server!.YgDomain}/api/texture/{textureEntity.Guid}";
        var userProfileEntity = await _db.UserProfiles.SingleAsync(x => x.Guid == req.UserId, ct);
        if (textureEntity.Texture == ProfileTextureModel.SkinTextureName)
        {
            userProfileEntity.SkinFileUrl = url;
        }

        if (textureEntity.Texture == ProfileTextureModel.CapeTextureName)
        {
            userProfileEntity.CapeFileUrl = url;
        }

        await _db.SaveChangesAsync(CancellationToken.None);
    }

    private byte[] ReadFile(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var bytes = new byte[file.Length];
        var read = stream.Read(bytes);
        if (read != bytes.Length)
        {
            throw new Exception("Cant read full img to mem");
        }

        return bytes;
    }

    private (int width, int height) ReadDimensions(byte[] file)
    {
        using var stream = new MemoryStream(file);
        var pngMeta = PngMetadataReader.ReadMetadata(stream);
        var ihdr = pngMeta
            .OfType<PngDirectory>()
            .First(x => x.GetPngChunkType() == PngChunkType.IHDR);
        var width = ihdr.GetObject(PngDirectory.TagImageWidth) as int?;
        var height = ihdr.GetObject(PngDirectory.TagImageHeight) as int?;
        if (width is null or > 64)
        {
            throw new NotSupportedException("Image width must be >= 1024");
        }

        if (height is null or > 64)
        {
            throw new NotSupportedException("Image height must be >= 1024");
        }

        return (width.Value, height.Value);
    }

    private string? GetAccessToken()
    {
        var auth = Request.Headers.Authorization.ToString();
        if (auth.Length < "Bearer ".Length)
            return null;

        var parts = auth.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts is not ["Bearer", _]
            ? null
            : parts[1];
    }
}