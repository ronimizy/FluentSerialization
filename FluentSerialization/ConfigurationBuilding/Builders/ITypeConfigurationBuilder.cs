using System.Linq.Expressions;

namespace FluentSerialization;

/// <typeparam name="T">Configured type</typeparam>
public interface ITypeConfigurationBuilder<T>
{
    /// <summary>
    ///     Configures a property (or field) serialization contract.
    /// </summary>
    /// <param name="expression">Member access expression of given property.</param>
    IPropertyConfigurationBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> expression);

    /// <summary>
    ///     Configures a property (or field) serialization contract.
    /// </summary>
    /// <param name="name">Property or field name</param>
    /// <returns></returns>
    IPropertyConfigurationBuilder<T, object> Property(string name);

    /// <summary>
    ///     Specifies given string value as SerializationTypeKey and DeserializationTypeKey for <typeparamref name="T" />.
    /// </summary>
    ITypeConfigurationBuilder<T> HasTypeKey(string key);

    /// <summary>
    ///     Specifies a string type key that will be bound when serializing the object of type <typeparamref name="T" />.
    /// </summary>
    ITypeConfigurationBuilder<T> HasSerializationTypeKey(string key);

    /// <summary>
    ///     Specifies a string type key that will be used to bind the serialized object to type <typeparamref name="T" />
    ///     when deserializing.
    /// </summary>
    ITypeConfigurationBuilder<T> HasDeserializationTypeKey(string key);

    /// <summary>
    ///     Specifies a property conversion
    /// </summary>
    /// <typeparam name="TConvert">A type that is will be converted to</typeparam>
    ITypeConfigurationBuilder<T> ConvertedWith<TConvert>(IConversion<T, TConvert> conversion);

    /// <summary>
    ///     Specifies a property conversion
    /// </summary>
    /// <typeparam name="TConvert">A type that is will be converted to</typeparam>
    ITypeConfigurationBuilder<T> ConvertedWith<TConvert>(Func<T, TConvert> convertTo, Func<TConvert, T> convertFrom);
}