using FluentSerialization.Extensions.NewtonsoftJson.Binding;
using Newtonsoft.Json;

namespace FluentSerialization.Extensions.NewtonsoftJson;

public static class ConfigurationExtensions
{
    /// <summary>
    ///     Converts specified configuration to a <see cref="JsonSerializerSettings" /> instance.
    /// </summary>
    public static JsonSerializerSettings AsNewtonsoftSerializationSettings(this IConfiguration configuration)
    {
        return new JsonSerializerSettings
        {
            SerializationBinder = new CustomSerializationBinder(configuration),
            ContractResolver = new CustomContractResolver(configuration),

            // needed to avoid formatting overhead and potential undefined behaviour, binder will provide the correct type key
            // and ReflectionUtils.cs:150 [string GetTypeName(Type, TypeNameAssemblyFormatHandling, ISerializationBinder?)]
            // should not mess with it
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            
            TypeNameHandling = TypeNameHandling.Auto,
        };
    }

    public static void ApplyToSerializationSettings(this IConfiguration configuration, JsonSerializerSettings settings)
    {
        settings.SerializationBinder = new CustomSerializationBinder(configuration);
        settings.ContractResolver = new CustomContractResolver(configuration);
        settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
        settings.TypeNameHandling = TypeNameHandling.Auto;
    }
}