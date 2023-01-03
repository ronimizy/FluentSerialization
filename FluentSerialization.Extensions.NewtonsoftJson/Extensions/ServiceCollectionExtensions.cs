using Microsoft.Extensions.DependencyInjection;

namespace FluentSerialization.Extensions.NewtonsoftJson;

public static class ServiceCollectionExtensions
{
    public static void AddNewtonsoftJson(this IExtendedSerializationConfiguration configuration)
    {
        var settings = configuration.Configuration.AsSettings();
        configuration.Collection.AddSingleton(settings);
    }
}