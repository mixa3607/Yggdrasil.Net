namespace ArkProjects.Minecraft.YggdrasilApi.Options;

/// <summary>
/// Web security options
/// </summary>
public class WebSecurityOptions
{
    /// <summary>
    /// Enable https redirection
    /// </summary>
    public bool EnableHttpsRedirections { get; set; } = true;

    /// <summary>
    /// Enable forwarded headers like X-Forwarded-For
    /// </summary>
    public bool EnableForwardedHeaders { get; set; } = true;
}