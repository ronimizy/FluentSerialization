using FluentSerialization.Tools;

namespace FluentSerialization.Implementations;

internal class Configuration : IConfiguration
{
    public Configuration(IReadOnlyCollection<ITypeConfiguration> types, FluentSerializationOptions options)
    {
        Types = types;
        ShouldEraseEnumerableType = options.ShouldEraseEnumerableType;
    }

    public IReadOnlyCollection<ITypeConfiguration> Types { get; }
    public bool ShouldEraseEnumerableType { get; }
}