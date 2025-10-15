using FluentScanning;
using FluentSerialization.Tools;

namespace FluentSerialization;

/// <summary>
///     Serialization contract configuration
/// </summary>
public interface ISerializationConfigurationBuilder
{
    /// <summary>
    ///     Configures the type serialization
    /// </summary>
    ITypeConfigurationBuilder<T> Type<T>();

    /// <summary>
    ///     Configures the type serialization
    /// </summary>
    ISerializationConfigurationBuilder Type<T>(Action<ITypeConfigurationBuilder<T>> configuration);

    ISerializationConfigurationBuilder Conversion<TSource, TDestination>(IConversion<TSource, TDestination> conversion);

    /// <summary>
    ///     Adds configurations from the specified assemblies
    /// </summary>
    ISerializationConfigurationBuilder AddConfigurationsFromAssemblies(params AssemblyProvider[] providers);

    /// <summary>
    ///     Configures options
    /// </summary>
    ISerializationConfigurationBuilder Options(Action<FluentSerializationOptions> options);
}
