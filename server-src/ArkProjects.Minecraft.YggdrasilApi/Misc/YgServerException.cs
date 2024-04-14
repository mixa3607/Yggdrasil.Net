using ArkProjects.Minecraft.YggdrasilApi.Models;

namespace ArkProjects.Minecraft.YggdrasilApi.Misc;

public class YgServerException : Exception
{
    public YgServerException(ErrorResponse response, Exception? innerException = null) : base(
        response.ErrorMessage, innerException)
    {
        Response = response;
    }

    public ErrorResponse Response { get; }
}