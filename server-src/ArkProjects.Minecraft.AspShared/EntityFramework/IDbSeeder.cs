using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.AspShared.EntityFramework;

public interface IDbSeeder<in T> where T : DbContext
{
    Task SeedAsync(T db, CancellationToken ct = default);
}
