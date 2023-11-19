using ArkProjects.Minecraft.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkProjects.Minecraft.Database.Configurators.Users;

public class RefreshTokenConfigurator : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasIndex(x => x.Token).IsUnique();
    }
}