namespace FluentSerialization;

/// <summary>
///     Visitor for locating type serialization key
/// </summary>
internal class SerializationKeyLocatingVisitor : ITypeConfigurationVisitor
{
    private readonly Type _type;

    public SerializationKeyLocatingVisitor(Type type)
    {
        _type = type;
    }

    public string? Key { get; private set; }

    public void Visit(ITypeConfiguration configuration)
    {
        if (configuration.Type == _type)
        {
            Key = configuration.SerializationKey;
        }
    }

    public void Visit(ITypeHierarchyConfiguration configuration)
    {
        if (configuration.Type == _type)
        {
            Key = configuration.SerializationKey;
            return;
        }

        if (configuration.Type.IsAssignableFrom(_type) is false)
            return;

        foreach (var child in configuration.Children)
        {
            child.Accept(this);

            if (Key is not null)
                return;
        }
    }
}