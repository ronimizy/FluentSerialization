using FluentSerialization.Exceptions;
using FluentSerialization.Internal;

namespace FluentSerialization.Implementations;

internal class ObjectPropertyConfigurationAdapter<THost, TProperty> :
    IPropertyConfigurationBuilder<THost, object>,
    IPropertyConfigurationBuilderInternal<THost>
{
    private readonly IPropertyConfigurationBuilderInternal<THost, TProperty> _builder;

    public ObjectPropertyConfigurationAdapter(IPropertyConfigurationBuilderInternal<THost, TProperty> builder)
    {
        _builder = builder;
    }

    public IPropertyConfigurationBuilder<THost, object> Called(string name)
    {
        _builder.Called(name);
        return this;
    }

    public void Ignored()
    {
        _builder.Ignored();
    }

    public IPropertyConfigurationBuilder<THost, object> ConvertedWith<T>(IConversion<object, T> conversion)
    {
        var adapter = new ObjectConversionAdapter<TProperty, T>(conversion);
        _builder.ConvertedWith(adapter);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, object> ConvertedWith<T>(
        Func<object, T> convertTo,
        Func<T, object> convertFrom)
    {
        var conversion = new DelegateConversion<object, T>(convertTo, convertFrom);
        return ConvertedWith(conversion);
    }

    public IPropertyConfigurationBuilder<THost, object> ShouldSerializeWhen(IValuePredicate<THost> predicate)
    {
        _builder.ShouldSerializeWhen(predicate);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, object> ShouldSerializeWhen(Func<THost, bool> predicate)
    {
        var valuePredicate = new DelegateValuePredicate<THost>(predicate);
        return ShouldSerializeWhen(valuePredicate);
    }

    public IPropertyConfigurationBuilder<THost, object> ShouldDeserializeWhen(IValuePredicate<THost> predicate)
    {
        _builder.ShouldDeserializeWhen(predicate);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, object> ShouldDeserializeWhen(Func<THost, bool> predicate)
    {
        var valuePredicate = new DelegateValuePredicate<THost>(predicate);
        return ShouldDeserializeWhen(valuePredicate);
    }

    public IPropertyConfigurationBuilder<THost, object> PositionedAt(int position)
    {
        _builder.PositionedAt(position);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, object> ShouldSpecifyType(bool specifyType)
    {
        _builder.ShouldSpecifyType(specifyType);
        return this;
    }

    public IPropertyConfigurationBuilder<THost, TCastProperty> Cast<TCastProperty>()
    {
        if (this is IPropertyConfigurationBuilder<THost, TCastProperty> builder)
            return builder;

        throw PropertyConfigurationBuilderException.InvalidBuilderType<THost, TProperty, TCastProperty>();
    }

    public IPropertyConfigurationBuilder<THost, object> EraseType()
        => this;

    public IPropertyConfiguration Build()
        => _builder.Build();
}