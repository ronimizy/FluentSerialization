namespace FluentSerialization.Implementations;

internal class ConversionProvider<TSource, TDestination> : IConversionProvider
{
    private readonly IConversion<TSource, TDestination> _conversion;

    public ConversionProvider(IConversion<TSource, TDestination> conversion)
    {
        _conversion = conversion;
    }

    public void Provide(IConversionConsumer consumer)
        => consumer.Consume(_conversion);
}