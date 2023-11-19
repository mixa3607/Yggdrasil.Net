using ArkProjects.Minecraft.AspShared.EntityFramework;
using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.YggdrasilApi.Services.UserPassword;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.YggdrasilApi.Services;

public class McDbContextSeeder : IDbSeeder<McDbContext>
{
    private readonly ILogger<McDbContextSeeder> _logger;
    private readonly IUserPasswordService _passwordService;

    public McDbContextSeeder(ILogger<McDbContextSeeder> logger, IUserPasswordService passwordService)
    {
        _logger = logger;
        _passwordService = passwordService;
    }

    public async Task SeedAsync(McDbContext db, CancellationToken ct = default)
    {
        //users
        if (!await db.Users.AnyAsync(ct))
        {
            _logger.LogInformation("Create admin user");
            var user = new UserEntity()
            {
                Permissions = new List<string>() {  },
                Login = "admin",
                PasswordHash = _passwordService.CreatePasswordHash("admin"),
                Email = "admin@test.com",
            };
            db.Users.Add(user);
            await db.SaveChangesAsync(CancellationToken.None);
        }
    }
}