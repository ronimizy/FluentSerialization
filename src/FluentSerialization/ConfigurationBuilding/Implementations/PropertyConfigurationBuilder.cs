using System.Reflection;
using FluentSerialization.Exceptions;
using FluentSerialization.Internal;
using FluentSerialization.Models;

namespace FluentSerialization.Implementations;

internal class PropertyConfigurationBuilder<THost, TProperty> : IPropertyConfigurationBuilderInternal<THost, TProperty>
{
    private readonly MemberInfo _info;
    private string _name;
    private ValueAccessMode _valueAccessMode;
    private int? _position;
    private bool? _shouldSpecifyType;
    private IConversionProvider? _conversionProvider;
    private IValuePredicateProvider? _serializationPredicateProvider;
    private IValuePredicateProvider? _deserializationPredicateProvider;

    public PropertyConfigurationBuilder(MemberInfo info)
    {
        _info = info;
        _name = info.Name;
        _valueAccessMode = ValueAccessMode.CanReadAndWrite;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> Called(string name)
    {
        _name = name;
        return this;
    }

    public void Ignored()
    {
        _valueAccessMode = ValueAccessMode.Ignore;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ConvertedWith<T>(IConversion<TProperty, T> conversion)
    {
        _conversionProvider = new ConversionProvider<TProperty, T>(conversion);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ConvertedWith<T>(
        Func<TProperty, T> convertTo,
        Func<T, TProperty> convertFrom)
    {
        var conversion = new DelegateConversion<TProperty, T>(convertTo, convertFrom);
        return ConvertedWith(conversion);
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ShouldSerializeWhen(IValuePredicate<THost> predicate)
    {
        _serializationPredicateProvider = new ValuePredicateProvider<THost>(predicate);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ShouldSerializeWhen(Func<THost, bool> predicate)
    {
        var valuePredicate = new DelegateValuePredicate<THost>(predicate);
        return ShouldSerializeWhen(valuePredicate);
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ShouldDeserializeWhen(IValuePredicate<THost> predicate)
    {
        _deserializationPredicateProvider = new ValuePredicateProvider<THost>(predicate);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ShouldDeserializeWhen(Func<THost, bool> predicate)
    {
        var valuePredicate = new DelegateValuePredicate<THost>(predicate);
        return ShouldDeserializeWhen(valuePredicate);
    }

    public IPropertyConfigurationBuilder<THost, TProperty> PositionedAt(int position)
    {
        _position = position;
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TProperty> ShouldSpecifyType(bool specifyType)
    {
        _shouldSpecifyType = specifyType;
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TCastProperty> Cast<TCastProperty>()
    {
        if (this is IPropertyConfigurationBuilder<THost, TCastProperty> builder)
            return builder;

        throw PropertyConfigurationBuilderException.InvalidBuilderType<THost, TProperty, TCastProperty>();
    }

    public IPropertyConfigurationBuilder<THost, object> EraseType()
    {
        if (this is IPropertyConfigurationBuilder<THost, object> builder)
            return builder;

        return new ObjectPropertyConfigurationAdapter<THost, TProperty>(this);
    }

    public IPropertyConfiguration Build()
    {
        return new PropertyConfiguration
        (
            _info,
            _name,
            _valueAccessMode,
            _position,
            _conversionProvider,
            _serializationPredicateProvider,
            _deserializationPredicateProvider,
            _shouldSpecifyType
        );
    }
}