namespace FluentSerialization.Exceptions;

public class PropertyConfigurationBuilderException : FluentSerializationException
{
    private PropertyConfigurationBuilderException(string message) : base(message) { }

    /// <summary>
    ///     Property configuration was cast to a an incompatible type.
    /// </summary>
    /// <typeparam name="THost">Type that declares a property</typeparam>
    /// <typeparam name="TActualProperty">Property type</typeparam>
    /// <typeparam name="TCastProperty">Type that attempted to cast property to</typeparam>
    internal static PropertyConfigurationBuilderException InvalidBuilderType<
        THost,
        TActualProperty,
        TCastProperty>()
    {
        return new PropertyConfigurationBuilderException(
            $"The builder type for property {typeof(THost).Name}.{typeof(TActualProperty).Name} " +
            $"is not assignable to {typeof(THost).Name}.{typeof(TCastProperty).Name}.");
    }
}