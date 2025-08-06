using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backpack.Infrastructure.Json;

public class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
    public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var cultureName = reader.GetString();

        if (string.IsNullOrWhiteSpace(cultureName))
        {
            throw new ArgumentException($"Invalid culture name");
        }

        return new CultureInfo(cultureName);
    }

    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            throw new ArgumentException($"Invalid culture name");
        }

        writer.WriteStringValue(value?.Name);
    }
}
