namespace FluentSerialization.Extensions;

public static class TypeExtensions
{
    /// <summary>
    ///     Determines if a <see cref="type"/> is constructed from <see cref="source"/>.
    ///     Constructed means that the <see cref="source"/> is an open generic type, and <see cref="type"/> is
    ///     derived from <see cref="source"/> that was closed by some type arguments  
    /// </summary>
    public static bool IsConstructedFrom(this Type type, Type source)
    {
        if (!type.IsConstructedGenericType)
            return type == source;

        var genericType = type.GetGenericTypeDefinition();
        return genericType == source;
    }
}