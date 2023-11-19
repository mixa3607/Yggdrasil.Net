namespace ArkProjects.Minecraft.YggdrasilApi.Options;

public class ServerNodeOptions
{
    public string[] BaseDomains { get; set; } = Array.Empty<string>();
    public string[] SkinDomains { get; set; } = Array.Empty<string>();
    public string Implementation { get; set; } = "";
    public string Version { get; set; } = "";
}