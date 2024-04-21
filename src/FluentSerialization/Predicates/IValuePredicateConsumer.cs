namespace FluentSerialization;

/// <summary>
///     Helper type to avoid complicated generic reflection
/// </summary>
public interface IValuePredicateConsumer
{
    void Consume<T>(IValuePredicate<T> predicate);
}