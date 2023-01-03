using FluentScanning;
using FluentSerialization.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace FluentSerialization.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds serialization configurations from specified assemblies
    /// </summary>
    public static IExtendedSerializationConfiguration AddFluentSerialization(
        this IServiceCollection collection,
        params AssemblyProvider[] providers)
    {
        var scanner = new AssemblyScanner(providers);

        ISerializationConfiguration[] configurations = scanner
            .ScanForTypesThat()
            .AreAssignableTo<ISerializationConfiguration>()
            .AreNotInterfaces()
            .AreNotAbstractClasses()
            .Select(x => (ISerializationConfiguration)Activator.CreateInstance(x))
            .ToArray();

        return collection.AddFluentSerialization(configurations, Enumerable.Empty<IConfigurationValidator>());
    }

    /// <summary>
    ///     Adds specified serialization configurations 
    /// </summary>
    public static IExtendedSerializationConfiguration AddFluentSerialization(
        this IServiceCollection collection,
        params ISerializationConfiguration[] configurations)
    {
        return collection.AddFluentSerialization(configurations, Enumerable.Empty<IConfigurationValidator>());
    }

    /// <summary>
    ///     Adds specified serialization configurations and validators
    /// </summary>
    public static IExtendedSerializationConfiguration AddFluentSerialization(
        this IServiceCollection collection,
        IEnumerable<ISerializationConfiguration> serializationConfigurations,
        IEnumerable<IConfigurationValidator> validators)
    {
        var configuration = ConfigurationBuilder.Build(serializationConfigurations, validators);
        collection.AddSingleton(configuration);

        return new ExtendedSerializationConfiguration(collection, configuration);
    }
}