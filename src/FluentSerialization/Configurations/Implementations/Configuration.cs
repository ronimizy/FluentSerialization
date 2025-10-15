using FluentSerialization.Tools;

namespace FluentSerialization.Implementations;

internal class Configuration : IConfiguration
{
    public Configuration(
        IReadOnlyCollection<ITypeConfiguration> types,
        FluentSerializationOptions options,
        IReadOnlyCollection<IConversionProvider> conversions)
    {
        Types = types;
        Conversions = conversions;
        ShouldEraseEnumerableType = options.ShouldEraseEnumerableType;
    }

    public IReadOnlyCollection<ITypeConfiguration> Types { get; }
    public IReadOnlyCollection<IConversionProvider> Conversions { get; }
    public bool ShouldEraseEnumerableType { get; }
}
