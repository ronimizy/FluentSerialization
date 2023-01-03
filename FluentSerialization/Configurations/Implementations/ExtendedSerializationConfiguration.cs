using Microsoft.Extensions.DependencyInjection;

namespace FluentSerialization.Implementations;

internal class ExtendedSerializationConfiguration : IExtendedSerializationConfiguration
{
    public ExtendedSerializationConfiguration(
        IServiceCollection collection,
        IConfiguration configuration)
    {
        Collection = collection;
        Configuration = configuration;
    }

    public IServiceCollection Collection { get; }
    public IConfiguration Configuration { get; }
}