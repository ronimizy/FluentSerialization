namespace FluentSerialization.Implementations;

internal class ValuePredicateProvider<T> : IValuePredicateProvider
{
    private readonly IValuePredicate<T> _predicate;

    public ValuePredicateProvider(IValuePredicate<T> predicate)
    {
        _predicate = predicate;
    }

    public void Provide(IValuePredicateConsumer consumer)
        => consumer.Consume(_predicate);
}