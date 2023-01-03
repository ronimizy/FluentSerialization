namespace FluentSerialization;

/// <summary>
///     Visitor for locating type deserialization key
/// </summary>
internal class DeserializationKeyLocatingVisitor : ITypeConfigurationVisitor
{
    private readonly string _key;

    public DeserializationKeyLocatingVisitor(string key)
    {
        _key = key;
    }

    public Type? Type { get; private set; }

    public void Visit(ITypeConfiguration configuration)
    {
        if (configuration.DeserializationKey.Equals(_key))
        {
            Type = configuration.Type;
        }
    }

    public void Visit(ITypeHierarchyConfiguration configuration)
    {
        if (configuration.DeserializationKey.Equals(_key))
        {
            Type = configuration.Type;
        }

        foreach (var child in configuration.Children)
        {
            child.Accept(this);

            if (Type is not null)
                return;
        }
    }
}