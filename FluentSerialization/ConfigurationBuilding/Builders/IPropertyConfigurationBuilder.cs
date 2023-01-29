namespace FluentSerialization;

/// <summary>
///     Property (or field) serialization contract configuration builder.
/// </summary>
/// <typeparam name="THost">A type that owns configured property</typeparam>
/// <typeparam name="TProperty">A type of configuring property</typeparam>
public interface IPropertyConfigurationBuilder<out THost, TProperty>
{
    /// <summary>
    ///     Specifies a property name, when serialized.
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> Called(string name);

    /// <summary>
    ///     Specifies that a property should be ignored
    /// </summary>
    void Ignored();

    /// <summary>
    ///     Specifies a property conversion
    /// </summary>
    /// <typeparam name="T">A type that is will be converted to</typeparam>
    IPropertyConfigurationBuilder<THost, TProperty> ConvertedWith<T>(IConversion<TProperty, T> conversion);

    /// <summary>
    ///     Specifies a property conversion
    /// </summary>
    /// <typeparam name="T">A type that is will be converted to</typeparam>
    IPropertyConfigurationBuilder<THost, TProperty> ConvertedWith<T>(
        Func<TProperty, T> convertTo,
        Func<T, TProperty> convertFrom);

    /// <summary>
    ///     Specifies a predicate that will be used to determine whether a property should be serialized
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> ShouldSerializeWhen(IValuePredicate<THost> predicate);

    /// <summary>
    ///     Specifies a predicate that will be used to determine whether a property should be serialized
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> ShouldSerializeWhen(Func<THost, bool> predicate);

    /// <summary>
    ///     Specifies a predicate that will be used to determine whether a property should be deserialized.
    ///     The data that has been already deserialized will be passed to the predicate.
    ///     If you want to use some data to evaluate the predicate, make sure it will be deserialized before this property,
    ///     by placing it before this property in type definition or by using <see cref="IPropertyConfigurationBuilder{THost,TProperty}.PositionedAt"/>
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> ShouldDeserializeWhen(IValuePredicate<THost> predicate);

    /// <summary>
    ///     Specifies a predicate that will be used to determine whether a property should be deserialized.
    ///     The data that has been already deserialized will be passed to the predicate.
    ///     If you want to use some data to evaluate the predicate, make sure it will be deserialized before this property,
    ///     by placing it before this property in type definition or by using <see cref="IPropertyConfigurationBuilder{THost,TProperty}.PositionedAt"/>
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> ShouldDeserializeWhen(Func<THost, bool> predicate);

    /// <summary>
    ///     Specifies a property position in serialized data.
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> PositionedAt(int position);
    
    /// <summary>
    ///     Specifies whether the property should have type explicitly specified in serialized data
    /// </summary>
    IPropertyConfigurationBuilder<THost, TProperty> ShouldSpecifyType(bool specifyType = true);
}