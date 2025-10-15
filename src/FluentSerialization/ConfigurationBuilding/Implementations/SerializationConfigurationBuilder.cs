using System.Reflection;
using FluentScanning;
using FluentSerialization.Binding;
using FluentSerialization.Binding.Implementations;
using FluentSerialization.Extensions;
using FluentSerialization.Internal;
using FluentSerialization.Tools;

namespace FluentSerialization.Implementations;

internal class SerializationConfigurationBuilder : ISerializationConfigurationBuilder
{
    private readonly Dictionary<Type, ITypeConfigurationBuilderInternal> _builders;
    private readonly List<IConversionProvider> _conversionProviders;
    private readonly FluentSerializationOptions _options;

    public SerializationConfigurationBuilder(FluentSerializationOptions options)
    {
        _options = options;
        _builders = [];
        _conversionProviders = [];
    }

    public IEnumerable<ITypeConfigurationBuilderInternal> Builders => _builders.Values;

    public IEnumerable<IConversionProvider> Conversions => _conversionProviders;

    public ITypeConfigurationBuilder<T> Type<T>()
        => GetOrAddBuilder<T>();

    public ISerializationConfigurationBuilder Type<T>(Action<ITypeConfigurationBuilder<T>> configuration)
    {
        ITypeConfigurationBuilder<T> builder = GetOrAddBuilder<T>();
        configuration(builder);

        return this;
    }

    public ISerializationConfigurationBuilder Conversion<TSource, TDestination>(
        IConversion<TSource, TDestination> conversion)
    {
        _conversionProviders.Add(new ConversionProvider<TSource, TDestination>(conversion));
        return this;
    }

    public ISerializationConfigurationBuilder AddConfigurationsFromAssemblies(params AssemblyProvider[] providers)
    {
        var scanner = new AssemblyScanner(providers);

        var binderOpenType = typeof(TypeConfigurationBinder<>);
        var configurationOpenType = typeof(ITypeSerializationConfiguration<>);

        var configurationTypes = scanner
            .ScanForTypesThat()
            .AreBasedOnTypesConstructedFrom(typeof(ITypeSerializationConfiguration<>));

        IEnumerable<(TypeInfo configurationType, Type type)> configurationsWithTypes = configurationTypes
            .SelectMany(x => x.ImplementedInterfaces, (t, i) => (configurationType: t, interfaceType: i))
            .Where(tuple => tuple.interfaceType.IsConstructedFrom(configurationOpenType))
            .Select(tuple => (tuple.configurationType, type: tuple.interfaceType.GenericTypeArguments.Single()));

        IEnumerable<(Type configurationType, Type type, Type binderType)> configurations = configurationsWithTypes
            .Select(tuple =>
            (
                configurationType: (Type)tuple.configurationType,
                tuple.type,
                binderType: binderOpenType.MakeGenericType(tuple.type)
            ));

        var openBuilderType = typeof(TypeConfigurationBuilder<>);

        foreach (var (configurationType, type, binderType) in configurations)
        {
            if (_builders.TryGetValue(type, out var builder) is false)
            {
                var builderType = openBuilderType.MakeGenericType(type);
                builder = (ITypeConfigurationBuilderInternal)Activator.CreateInstance(builderType);

                _builders.Add(type, builder);
            }

            var configuration = Activator.CreateInstance(configurationType);
            var binder = (ITypeConfigurationBinder)Activator.CreateInstance(binderType, configuration, builder);
            binder.Bind();
        }

        return this;
    }

    public ISerializationConfigurationBuilder Options(Action<FluentSerializationOptions> options)
    {
        options.Invoke(_options);
        return this;
    }

    private ITypeConfigurationBuilder<T> GetOrAddBuilder<T>()
    {
        var type = typeof(T);

        if (_builders.TryGetValue(type, out var builder) is false)
        {
            builder = new TypeConfigurationBuilder<T>();
            _builders.Add(type, builder);
        }

        return builder.Cast<T>();
    }
}
