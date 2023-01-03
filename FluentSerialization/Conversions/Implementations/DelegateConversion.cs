namespace FluentSerialization.Implementations;

internal class DelegateConversion<TSource, TDestination> : IConversion<TSource, TDestination>
{
    private readonly Func<TSource, TDestination> _convertTo;
    private readonly Func<TDestination, TSource> _convertFrom;

    public DelegateConversion(Func<TSource, TDestination> convertTo, Func<TDestination, TSource> convertFrom)
    {
        _convertTo = convertTo;
        _convertFrom = convertFrom;
    }

    public TDestination ConvertTo(TSource source)
        => _convertTo.Invoke(source);

    public TSource ConvertFrom(TDestination destination)
        => _convertFrom.Invoke(destination);
}