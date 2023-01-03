using Microsoft.Extensions.DependencyInjection;

namespace FluentSerialization;

/// <summary>
///     Serialization contract extension for service collection registration
/// </summary>
public interface IExtendedSerializationConfiguration
{
    IServiceCollection Collection { get; }

    IConfiguration Configuration { get; }
}