using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiBureau.JPMorgan.Api.Converters;

public class JsonStringToDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && decimal.TryParse(reader.GetString(), out var result))
            return result;

        throw new JsonException($"Invalid value for decimal: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}