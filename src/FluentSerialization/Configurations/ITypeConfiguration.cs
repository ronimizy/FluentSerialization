namespace FluentSerialization;

/// <summary>
///     Type serialization contract
/// </summary>
public interface ITypeConfiguration
{
    Type Type { get; }

    /// <summary>
    ///     A string type key that will be bound when serializing the object of given type.
    /// </summary>
    string SerializationKey { get; }
    
    /// <summary>
    ///     Specifies a string type key that will be used to bind the serialized object to type when deserializing.
    /// </summary>
    string DeserializationKey { get; }

    /// <summary>
    ///     Properties
    /// </summary>
    IReadOnlyCollection<IPropertyConfiguration> PropertyConfigurations { get; }

    void Accept(ITypeConfigurationVisitor visitor);

    void AcceptConversionConsumer(IConversionConsumer consumer);
}