namespace FluentSerialization;

/// <summary>
///     Helper type to avoid complicated generic reflection
/// </summary>
public interface IValuePredicateProvider
{
    void Provide(IValuePredicateConsumer consumer);
}