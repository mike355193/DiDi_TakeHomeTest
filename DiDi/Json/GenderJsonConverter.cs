using System.Text.Json;
using System.Text.Json.Serialization;
using DiDi.Enums;

namespace DiDi.Json;

public sealed class GenderJsonConverter : JsonConverter<Gender>
{
    public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Gender must be 男 or 女.");
        }

        return reader.GetString() switch
        {
            "男" => Gender.Male,
            "女" => Gender.Female,
            _ => throw new JsonException("Gender must be 男 or 女.")
        };
    }

    public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value == Gender.Male ? "男" : "女");
    }
}
