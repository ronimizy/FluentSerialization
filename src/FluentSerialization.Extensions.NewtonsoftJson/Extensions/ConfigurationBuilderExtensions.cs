using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FluentSerialization.Extensions.NewtonsoftJson;

public static class ConfigurationBuilderExtensions
{
    public static OptionsBuilder<JsonSerializerSettings> UseFluentSerialization(
        this OptionsBuilder<JsonSerializerSettings> builder)
    {
        return builder.Configure<IConfiguration>(
            (settings, configuration) => configuration.ApplyToSerializationSettings(settings));
    }
}