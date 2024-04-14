using ArkProjects.Minecraft.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkProjects.Minecraft.Database.Configurators.Users;

public class ServerConfigurator : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(EntityTypeBuilder<ServerEntity> builder)
    {
        builder.HasIndex(x => new { x.YgDomain }).IsUnique();
    }
}