using System.Reflection;
using FluentSerialization.Models;

namespace FluentSerialization.Implementations;

internal class PropertyConfiguration : IPropertyConfiguration
{
    private readonly IConversionProvider? _conversionProvider;
    private readonly IValuePredicateProvider? _serializationFilterProvider;
    private readonly IValuePredicateProvider? _deserializationFilterProvider;

    public PropertyConfiguration(
        MemberInfo info,
        string name,
        ValueAccessMode accessMode,
        int? position,
        IConversionProvider? conversionProvider,
        IValuePredicateProvider? serializationFilterProvider,
        IValuePredicateProvider? deserializationFilterProvider)
    {
        Info = info;
        Name = name;
        AccessMode = accessMode;
        _conversionProvider = conversionProvider;
        _serializationFilterProvider = serializationFilterProvider;
        _deserializationFilterProvider = deserializationFilterProvider;
        Position = position;
    }

    public MemberInfo Info { get; }
    public string Name { get; }
    public ValueAccessMode AccessMode { get; }
    public int? Position { get; }

    public void Accept(IConversionConsumer consumer)
        => _conversionProvider?.Provide(consumer);

    public void AcceptSerializationFilterConsumer(IValuePredicateConsumer consumer)
        => _serializationFilterProvider?.Provide(consumer);

    public void AcceptDeserializationFilterConsumer(IValuePredicateConsumer consumer)
        => _deserializationFilterProvider?.Provide(consumer);
}