using System.ComponentModel.DataAnnotations;

namespace ArkProjects.Minecraft.AspShared.EntityFramework;

/// <summary>
/// pg db options
/// </summary>
public class PgDatabaseOptions
{
    /// <summary>
    /// Connection string
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = "";
}