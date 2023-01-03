namespace FluentSerialization;

/// <summary>
///     Visitor for location configuration for <see cref="_type"/>
/// </summary>
internal class ConfigurationLocatingVisitor : ITypeConfigurationVisitor
{
    private readonly Type _type;

    public ConfigurationLocatingVisitor(Type type)
    {
        _type = type;
    }
    
    public ITypeConfiguration? Configuration { get; private set; }

    public void Visit(ITypeConfiguration configuration)
    {
        if (configuration.Type == _type)
        {
            Configuration = configuration;
        }
    }

    public void Visit(ITypeHierarchyConfiguration configuration)
    {
        if (configuration.Type == _type)
        {
            Configuration = configuration;
            return;
        }
        
        if (configuration.Type.IsAssignableFrom(_type) is false)
            return;

        foreach (var child in configuration.Children)
        {
            child.Accept(this);
            
            if (Configuration is not null)
                return;
        }
    }
}