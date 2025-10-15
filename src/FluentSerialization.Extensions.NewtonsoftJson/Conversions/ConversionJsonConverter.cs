using Newtonsoft.Json;

namespace FluentSerialization.Extensions.NewtonsoftJson.Conversions;

internal class ConversionJsonConverter<TSource, TDestination> : JsonConverter<TSource>
{
    private readonly IConversion<TSource, TDestination> _conversion;

    public ConversionJsonConverter(IConversion<TSource, TDestination> conversion)
    {
        _conversion = conversion;
    }

    public override void WriteJson(JsonWriter writer, TSource? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            serializer.Serialize(writer, value);
        }
        else
        {
            TDestination convertedValue = _conversion.ConvertTo(value);
            serializer.Serialize(writer, convertedValue);
        }
    }

    public override TSource? ReadJson(
        JsonReader reader,
        Type objectType,
        TSource? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        TDestination? convertedValue = serializer.Deserialize<TDestination>(reader);
        return convertedValue is null ? default : _conversion.ConvertFrom(convertedValue);
    }
}
