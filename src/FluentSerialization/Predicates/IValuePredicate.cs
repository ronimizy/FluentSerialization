namespace FluentSerialization;

/// <summary>
///     Predicated over some value
/// </summary>
/// <typeparam name="T">Value type</typeparam>
public interface IValuePredicate<in T>
{
    bool Match(T value);
}