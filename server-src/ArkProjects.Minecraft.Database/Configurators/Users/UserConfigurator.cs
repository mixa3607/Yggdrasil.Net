using ArkProjects.Minecraft.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkProjects.Minecraft.Database.Configurators.Users;

public class UserConfigurator : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasIndex(x => new { x.Guid }).IsUnique();
        builder.HasIndex(x => new { x.LoginNormalized, x.DeletedAt }).IsUnique();
    }
}