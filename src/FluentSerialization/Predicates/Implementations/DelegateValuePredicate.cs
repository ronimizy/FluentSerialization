namespace FluentSerialization.Implementations;

internal class DelegateValuePredicate<T> : IValuePredicate<T>
{
    private readonly Func<T, bool> _predicate;

    public DelegateValuePredicate(Func<T, bool> predicate)
    {
        _predicate = predicate;
    }

    public bool Match(T value)
        => _predicate.Invoke(value);
}