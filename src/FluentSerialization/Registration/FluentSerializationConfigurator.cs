using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FluentSerialization.Registration;

internal class FluentSerializationConfigurator : IFluentSerializationConfigurator
{
    private readonly IServiceCollection _collection;

    public FluentSerializationConfigurator(IServiceCollection collection)
    {
        _collection = collection;
    }

    public IFluentSerializationConfigurator WithConfiguration<TConfiguration>()
        where TConfiguration : class, ISerializationConfiguration
    {
        _collection.TryAddSingleton<ISerializationConfiguration, TConfiguration>();
        return this;
    }

    public IFluentSerializationConfigurator WithValidator<TValidator>() where TValidator : class, IConfigurationValidator
    {
        _collection.TryAddSingleton<IConfigurationValidator, TValidator>();
        return this;
    }
}