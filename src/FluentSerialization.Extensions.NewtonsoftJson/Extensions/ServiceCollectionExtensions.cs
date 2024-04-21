using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FluentSerialization.Extensions.NewtonsoftJson;

public static class ServiceCollectionExtensions
{
    public static void AddNewtonsoftJson(this IExtendedSerializationConfiguration configuration)
    {
        JsonSerializerSettings settings = configuration.Configuration.AsNewtonsoftSerializationSettings();
        configuration.Collection.AddSingleton(settings);
    }
}