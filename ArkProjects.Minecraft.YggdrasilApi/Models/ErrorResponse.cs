using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Models;

public class ErrorResponse
{
    [JsonProperty("error")]
    public required string Error { get; set; }

    [JsonProperty("errorMessage")]
    public required string ErrorMessage { get; set; }

    [JsonProperty("cause")]
    public string? Cause { get; set; }

    [JsonIgnore]
    public int StatusCode { get; set; }
}