﻿namespace ArkProjects.Minecraft.Database.Entities.Users;

public class RefreshTokenEntity
{
    public long Id { get; set; }
    public required string Token { get; set; }

    public required Guid UserGuid { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset ExpiredAt { get; set; }
    public string? UserAgent { get; set; }
    public string? Fingerprint { get; set; }

    public DateTimeOffset? UsedAt { get; set; }
}
