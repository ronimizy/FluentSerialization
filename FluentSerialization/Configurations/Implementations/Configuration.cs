namespace FluentSerialization.Implementations;

internal class Configuration : IConfiguration
{
    public Configuration(IReadOnlyCollection<ITypeConfiguration> types)
    {
        Types = types;
    }

    public IReadOnlyCollection<ITypeConfiguration> Types { get; }
}