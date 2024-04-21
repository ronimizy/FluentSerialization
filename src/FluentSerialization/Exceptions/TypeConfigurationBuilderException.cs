using System.Linq.Expressions;
using System.Reflection;

namespace FluentSerialization.Exceptions;

public class TypeConfigurationBuilderException : FluentSerializationException
{
    private TypeConfigurationBuilderException(string message) : base(message) { }

    /// <summary>
    ///     Property access expression is not supported by the builder.
    /// </summary>
    internal static TypeConfigurationBuilderException UnsupportedPropertyAccessExpression(Expression expression)
        => new TypeConfigurationBuilderException($"Unsupported property access expression: {expression}");

    /// <summary>
    ///     Member is not supported by the builder.
    /// </summary>
    internal static TypeConfigurationBuilderException InvalidExpressionMember(MemberInfo info)
        => new TypeConfigurationBuilderException($"Invalid expression member accessed: {info}");

    /// <summary>
    ///     Specified expression has a unsupported host body type.
    /// </summary>
    internal static TypeConfigurationBuilderException InvalidExpressionHost(Expression expression)
        => new TypeConfigurationBuilderException($"Invalid expression host: {expression}");

    /// <summary>
    ///     Type configuration was cast to a an incompatible type.
    /// </summary>
    /// <typeparam name="TActual"></typeparam>
    /// <typeparam name="TCast"></typeparam>
    /// <returns></returns>
    internal static TypeConfigurationBuilderException InvalidBuilderType<TActual, TCast>()
    {
        var message = $"Invalid builder type. Expected: {typeof(TCast)}, actual: {typeof(TActual)}";
        return new TypeConfigurationBuilderException(message);
    }

    /// <summary>
    ///     Member called <see cref="name"/> is not supported by the builder.
    /// </summary>
    /// <param name="name">Member name</param>
    /// <typeparam name="T">Configured type</typeparam>
    internal static TypeConfigurationBuilderException InvalidMember<T>(string name)
        => new TypeConfigurationBuilderException($"Invalid member: {typeof(T)}.{name}");
}