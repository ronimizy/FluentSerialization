namespace FluentSerialization;

/// <summary>
///     Interface for configuring a type serialization contract
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITypeSerializationConfiguration<T>
{
    void Configure(ITypeConfigurationBuilder<T> builder);
}