namespace FluentSerialization.Implementations;

internal class TypeConfiguration : ITypeConfiguration
{
    private readonly IConversionProvider? _conversionProvider;

    public TypeConfiguration(
        Type type,
        string serializationKey,
        string deserializationKey,
        IReadOnlyCollection<IPropertyConfiguration> propertyConfigurations,
        IConversionProvider? conversionProvider)
    {
        Type = type;
        SerializationKey = serializationKey;
        DeserializationKey = deserializationKey;
        PropertyConfigurations = propertyConfigurations;
        _conversionProvider = conversionProvider;
    }

    public Type Type { get; }
    public string SerializationKey { get; }
    public string DeserializationKey { get; }
    public IReadOnlyCollection<IPropertyConfiguration> PropertyConfigurations { get; }

    public void Accept(ITypeConfigurationVisitor visitor)
        => visitor.Visit(this);

    public void AcceptConversionConsumer(IConversionConsumer consumer)
        => _conversionProvider?.Provide(consumer);
}