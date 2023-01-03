using Newtonsoft.Json;

namespace FluentSerialization.Extensions.NewtonsoftJson.Conversions;

internal class ConversionConsumer : IConversionConsumer
{
    public JsonConverter? Converter { get; private set; }

    public void Consume<TSource, TDestination>(IConversion<TSource, TDestination> conversion)
    {
        Converter = new ConversionJsonConverter<TSource, TDestination>(conversion);
    }
}