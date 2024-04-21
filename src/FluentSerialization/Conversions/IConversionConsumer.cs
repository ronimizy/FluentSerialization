namespace FluentSerialization;

/// <summary>
///     Helper type to avoid complicated generic reflection
/// </summary>
public interface IConversionConsumer
{
    void Consume<TSource, TDestination>(IConversion<TSource, TDestination> conversion);
}