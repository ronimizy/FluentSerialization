namespace FluentSerialization.Extensions;

public static class SerializationConfigurationExtensions
{
    /// <summary>
    ///     Builds a configuration with default validators
    /// </summary>
    public static IConfiguration Build(this ISerializationConfiguration configuration)
        => SerializationConfigurationFactory.Build(configuration);
}