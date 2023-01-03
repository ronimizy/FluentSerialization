namespace FluentSerialization;

/// <summary>
///     Value conversion
/// </summary>
/// <typeparam name="TSource">Source value type</typeparam>
/// <typeparam name="TDestination">Destination value type</typeparam>
public interface IConversion<TSource, TDestination>
{
    TDestination ConvertTo(TSource source);
    
    TSource ConvertFrom(TDestination destination);
}