namespace ArkProjects.Minecraft.AspShared.Validation;

[AttributeUsage(AttributeTargets.Parameter)]
public class SkipValidatorsCheckAttribute : Attribute
{
    public string Reason { get; }

    public SkipValidatorsCheckAttribute(string reason)
    {
        Reason = reason;
    }
}