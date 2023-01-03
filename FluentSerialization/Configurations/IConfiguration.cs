namespace FluentSerialization;

/// <summary>
///     Serialization contract
/// </summary>
public interface IConfiguration
{
    IReadOnlyCollection<ITypeConfiguration> Types { get; }
}