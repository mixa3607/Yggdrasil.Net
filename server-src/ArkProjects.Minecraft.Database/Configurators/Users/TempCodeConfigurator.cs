using ArkProjects.Minecraft.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkProjects.Minecraft.Database.Configurators.Users;

public class TempCodeConfigurator : IEntityTypeConfiguration<TempCodeEntity>
{
    public void Configure(EntityTypeBuilder<TempCodeEntity> builder)
    {
    }
}