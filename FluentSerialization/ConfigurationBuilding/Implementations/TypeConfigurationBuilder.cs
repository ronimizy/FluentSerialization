using System.Linq.Expressions;
using System.Reflection;
using FluentSerialization.Exceptions;
using FluentSerialization.Internal;

namespace FluentSerialization.Implementations;

internal class TypeConfigurationBuilder<T> : ITypeConfigurationBuilder<T>, ITypeConfigurationBuilderInternal
{
    const BindingFlags DefaultBindingFlags = BindingFlags.Instance |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic;

    private readonly Type _type;
    private readonly Dictionary<MemberInfo, IPropertyConfigurationBuilderInternal<T>> _propertyConfigurations;

    private string _serializationKey;
    private string _deserializationKey;
    private IConversionProvider? _conversionProvider;

    public TypeConfigurationBuilder()
    {
        _type = typeof(T);
        _propertyConfigurations = new Dictionary<MemberInfo, IPropertyConfigurationBuilderInternal<T>>();

        _serializationKey = _type.AssemblyQualifiedName ?? string.Empty;
        _deserializationKey = _type.AssemblyQualifiedName ?? string.Empty;
    }

    public IPropertyConfigurationBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
            throw TypeConfigurationBuilderException.UnsupportedPropertyAccessExpression(expression);

        if (memberExpression.Member is not (PropertyInfo or FieldInfo))
            throw TypeConfigurationBuilderException.InvalidExpressionMember(memberExpression.Member);

        var memberInfo = memberExpression.Member;

        if (memberExpression.Expression is not ParameterExpression)
            throw TypeConfigurationBuilderException.InvalidExpressionHost(memberExpression.Expression);

        if (_propertyConfigurations.TryGetValue(memberInfo, out var builder) is false)
        {
            builder = new PropertyConfigurationBuilder<T, TProperty>(memberInfo);
            _propertyConfigurations.Add(memberInfo, builder);
        }

        return builder.Cast<TProperty>();
    }

    public IPropertyConfigurationBuilder<T, object> Property(string name)
    {
        var type = typeof(T);

        MemberInfo[] memberInfo = type.GetMember(name, DefaultBindingFlags);

        if (memberInfo.Length is not 1)
            throw TypeConfigurationBuilderException.InvalidMember<T>(name);

        var info = memberInfo[0];

        if (info is not (PropertyInfo or FieldInfo))
            throw TypeConfigurationBuilderException.InvalidMember<T>(name);

        if (_propertyConfigurations.TryGetValue(info, out var builder) is false)
        {
            builder = new PropertyConfigurationBuilder<T, object>(info);
            _propertyConfigurations.Add(info, builder);
        }

        return builder.EraseType();
    }

    public ITypeConfigurationBuilder<T> HasTypeKey(string key)
    {
        _serializationKey = key;
        _deserializationKey = key;

        return this;
    }

    public ITypeConfigurationBuilder<T> HasSerializationTypeKey(string key)
    {
        _serializationKey = key;
        return this;
    }

    public ITypeConfigurationBuilder<T> HasDeserializationTypeKey(string key)
    {
        _deserializationKey = key;
        return this;
    }

    public ITypeConfigurationBuilder<T> ConvertedWith<TConvert>(IConversion<T, TConvert> conversion)
    {
        _conversionProvider = new ConversionProvider<T, TConvert>(conversion);
        return this;
    }

    public ITypeConfigurationBuilder<T> ConvertedWith<TConvert>(
        Func<T, TConvert> convertTo,
        Func<TConvert, T> convertFrom)
    {
        var conversion = new DelegateConversion<T, TConvert>(convertTo, convertFrom);
        return ConvertedWith(conversion);
    }

    public ITypeConfigurationBuilder<TCast> Cast<TCast>()
    {
        if (this is ITypeConfigurationBuilder<TCast> builder)
            return builder;

        throw TypeConfigurationBuilderException.InvalidBuilderType<T, TCast>();
    }

    public ITypeConfiguration Build()
    {
        IPropertyConfiguration[] properties = _propertyConfigurations.Values
            .Select(x => x.Build())
            .ToArray();

        return new TypeConfiguration(_type, _serializationKey, _deserializationKey, properties, _conversionProvider);
    }
}