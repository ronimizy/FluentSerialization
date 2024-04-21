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
    void Type<T>(Action<ITypeConfigurationBuilder<T>> configuration);

    /// <summary>
    ///     Adds configurations from the specified assemblies
    /// </summary>
    void AddConfigurationsFromAssemblies(params AssemblyProvider[] providers);
    
    /// <summary>
    ///     Configures options
    /// </summary>
    void Options(Action<FluentSerializationOptions> options);
}