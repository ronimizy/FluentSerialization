using System.Reflection;
using FluentSerialization.Models;

namespace FluentSerialization;

/// <summary>
///     Interface for property serialization contract
/// </summary>
public interface IPropertyConfiguration
{
    /// <summary>
    ///     Property type info
    /// </summary>
    MemberInfo Info { get; }

    /// <summary>
    ///     Property name used in serialized data
    /// </summary>
    string Name { get; }

    ValueAccessMode AccessMode { get; }

    /// <summary>
    ///     Position in serialized data
    /// </summary>
    public int? Position { get; }
    
    /// <summary>
    ///     Specifies whether the property should have type explicitly specified in serialized data
    /// </summary>
    public bool? SpecifyType { get; }
    
    /// <summary>
    ///     Specifies whether the property should be ignored when null
    /// </summary>
    public bool? IgnoreNulls { get; }

    void Accept(IConversionConsumer consumer);

    void AcceptSerializationFilterConsumer(IValuePredicateConsumer consumer);
    void AcceptDeserializationFilterConsumer(IValuePredicateConsumer consumer);
}