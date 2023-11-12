using Newtonsoft.Json;

namespace ArkProjects.Minecraft.YggdrasilApi.Misc.JsonConverters;

public class YggdrasilGuidConverter : JsonConverter<Guid>
{
    public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("N"));
    }

    public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var str = reader.ReadAsString();
        return str == null ? existingValue : Guid.Parse(str);
    }
}