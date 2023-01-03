namespace FluentSerialization.Implementations;

internal class TypeHierarchyConfigurationProxy : ITypeHierarchyConfiguration
{
    private readonly ITypeConfiguration _configuration;

    public TypeHierarchyConfigurationProxy(
        ITypeConfiguration configuration,
        IReadOnlyCollection<ITypeConfiguration> children)
    {
        _configuration = configuration;
        Children = children;
    }

    public Type Type => _configuration.Type;
    public string SerializationKey => _configuration.SerializationKey;
    public string DeserializationKey => _configuration.DeserializationKey;
    public IReadOnlyCollection<IPropertyConfiguration> PropertyConfigurations => _configuration.PropertyConfigurations;
    public IReadOnlyCollection<ITypeConfiguration> Children { get; }

    public void Accept(ITypeConfigurationVisitor visitor)
        => visitor.Visit(this);

    public void AcceptConversionConsumer(IConversionConsumer consumer)
        => _configuration.AcceptConversionConsumer(consumer);
}