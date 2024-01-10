using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAPI.Converters;

/// <summary>DateOnly型コンバーター</summary>
public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.FromDateTime(reader.GetDateTime());
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        var isoDate = $"{value.ToString("yyyy-MM-dd")}T00:00:00+09:00";
        writer.WriteStringValue(isoDate);
    }
}