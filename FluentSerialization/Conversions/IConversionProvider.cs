namespace FluentSerialization;

/// <summary>
///     Helper type to avoid complicated generic reflection
/// </summary>
public interface IConversionProvider
{
    void Provide(IConversionConsumer consumer);
}