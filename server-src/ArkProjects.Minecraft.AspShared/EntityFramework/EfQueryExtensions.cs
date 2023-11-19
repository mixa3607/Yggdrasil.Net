namespace ArkProjects.Minecraft.AspShared.EntityFramework;

public static class EfQueryExtensions
{
    public static IQueryable<T> IsDeleted<T>(this IQueryable<T> query, bool? isDeleted) where T : IEntityWithDeletingFlag
    {
        return isDeleted switch
        {
            null => query,
            true => query.Where(x => x.DeletedAt != null),
            false => query.Where(x => x.DeletedAt == null),
        };
    }
}