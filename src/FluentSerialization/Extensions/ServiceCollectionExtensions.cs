using FluentSerialization.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentSerialization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentSerialization(
        this IServiceCollection collection,
        Action<IFluentSerializationConfigurator> configuration)
    {
        var configurator = new FluentSerializationConfigurator(collection);
        configuration.Invoke(configurator);

        collection.AddSingleton<IConfiguration>(provider => SerializationConfigurationFactory.Build(
            configurations: provider.GetRequiredService<IEnumerable<ISerializationConfiguration>>(),
            validators: provider.GetRequiredService<IEnumerable<IConfigurationValidator>>()));

        return collection;
    }
}