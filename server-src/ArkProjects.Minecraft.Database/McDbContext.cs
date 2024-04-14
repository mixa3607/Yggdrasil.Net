using ArkProjects.Minecraft.Database.Entities;
using ArkProjects.Minecraft.Database.Entities.Users;
using ArkProjects.Minecraft.Database.Entities.Yg;
using Microsoft.EntityFrameworkCore;

namespace ArkProjects.Minecraft.Database;

public class McDbContext : DbContext
{
    //auth
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<UserAccessTokenEntity> UserAccessTokens { get; set; } = null!;
    public DbSet<TempCodeEntity> TempCodes { get; set; } = null!;
    public DbSet<UserProfileEntity> UserProfiles { get; set; } = null!;
    public DbSet<UserServerJoinEntity> UserServerJoins { get; set; } = null!;
    public DbSet<TextureEntity> Textures { get; set; } = null!;


    public DbSet<ServerEntity> Servers { get; set; } = null!;


    public McDbContext(DbContextOptions<McDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}