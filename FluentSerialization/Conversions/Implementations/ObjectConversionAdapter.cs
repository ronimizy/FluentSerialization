namespace FluentSerialization.Implementations;

public class ObjectConversionAdapter<TSource, TDestination> : IConversion<TSource, TDestination>
{
    private readonly IConversion<object, TDestination> _conversion;

    public ObjectConversionAdapter(IConversion<object, TDestination> conversion)
    {
        _conversion = conversion;
    }

    public TDestination ConvertTo(TSource source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _conversion.ConvertTo(source);
    }

    public TSource ConvertFrom(TDestination destination)
    {
        var value = _conversion.ConvertFrom(destination);

        if (value is TSource source)
            return source;

        var message = $"The conversion from {typeof(TDestination)} to {typeof(TSource)} returned an object of type {value.GetType()}.";
        throw new ArgumentException(message);
    }
}