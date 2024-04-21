namespace FluentSerialization.Extensions.NewtonsoftJson.Predicates;

internal class ObjectPredicateConsumer : IValuePredicateConsumer
{
    public Predicate<object>? Predicate { get; private set; }

    public void Consume<T>(IValuePredicate<T> predicate)
    {
        Predicate = obj => obj is T value && predicate.Match(value);
    }
}