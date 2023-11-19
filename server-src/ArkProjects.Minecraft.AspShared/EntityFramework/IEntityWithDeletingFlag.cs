namespace ArkProjects.Minecraft.AspShared.EntityFramework;

public interface IEntityWithDeletingFlag
{
    DateTimeOffset? DeletedAt { get; }
}