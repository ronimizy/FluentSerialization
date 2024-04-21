namespace FluentSerialization;

/// <summary>
///     Interface for configuring a serialization contract
/// </summary>
public interface ISerializationConfiguration
{
    void Configure(ISerializationConfigurationBuilder configurationBuilder);
}