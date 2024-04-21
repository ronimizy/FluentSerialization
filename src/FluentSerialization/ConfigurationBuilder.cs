using FluentSerialization.Exceptions;

namespace FluentSerialization;

/// <summary>
/// <see cref=""/>
///     Helper class for configuration building 
/// </summary>
[Obsolete(message: "Use SerializationConfigurationFactory", error: true)]
public static class ConfigurationBuilder
{
    /// <summary>
    ///     Builds a configuration from given delegate
    /// </summary>
    public static IConfiguration Build(Action<ISerializationConfigurationBuilder> action)
        => SerializationConfigurationFactory.Build(action);

    /// <summary>
    ///     Builds a configuration from given serialization configurations
    /// </summary>
    public static IConfiguration Build(params ISerializationConfiguration[] configurations)
        => SerializationConfigurationFactory.Build(configurations);

    /// <summary>
    ///     Builds a configuration from given serialization configurations and validators
    /// </summary>
    /// <exception cref="SerializationConfigurationException">Configuration is invalid</exception>
    public static IConfiguration Build(
        IEnumerable<ISerializationConfiguration> configurations,
        IEnumerable<IConfigurationValidator> validators)
    {
        return SerializationConfigurationFactory.Build(configurations, validators);
    }
}