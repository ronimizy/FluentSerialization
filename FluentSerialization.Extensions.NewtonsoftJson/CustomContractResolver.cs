using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using FluentSerialization.Extensions.NewtonsoftJson.Conversions;
using FluentSerialization.Extensions.NewtonsoftJson.Predicates;
using FluentSerialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FluentSerialization.Extensions.NewtonsoftJson;

internal class CustomContractResolver : DefaultContractResolver
{
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<Type, JsonContract> _contractCache;
    private readonly IContractResolver? _resolver;

    public CustomContractResolver(IConfiguration configuration, IContractResolver? resolver)
    {
        _configuration = configuration;
        _resolver = resolver;
        _contractCache = new ConcurrentDictionary<Type, JsonContract>();
    }

    public override JsonContract ResolveContract(Type type)
    {
        return _contractCache.GetOrAdd(type,
            _ =>
            {
                var visitor = new ConfigurationLocatingVisitor(type);

                foreach (var configuration in _configuration.Types)
                {
                    configuration.Accept(visitor);
                }

                var typeConfiguration = visitor.Configuration;

                return typeConfiguration is null ? ResolveContractDefault(type) : ResolveContract(typeConfiguration);
            });
    }

    private JsonContract ResolveContractDefault(Type type)
    {
        var contract = _resolver?.ResolveContract(type) ?? base.ResolveContract(type);

        if (_configuration.ShouldEraseEnumerableType is false)
            return contract;

        if (contract is not JsonObjectContract objectContract)
            return contract;

        foreach (var property in objectContract.Properties)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                property.TypeNameHandling = TypeNameHandling.None;
        }

        return contract;
    }

    private JsonContract ResolveContract(ITypeConfiguration configuration)
    {
        var contract = CreateObjectContract(configuration.Type);

        foreach (var property in contract.Properties)
        {
            var propertyConfiguration = configuration.PropertyConfigurations
                .FirstOrDefault(x => property.UnderlyingName?.Equals(x.Info.Name) is true);

            if (propertyConfiguration is not null)
                ResolveProperty(propertyConfiguration, property);
        }

        IEnumerable<IPropertyConfiguration> propertiesToAdd = configuration.PropertyConfigurations
            .Where(propertyConfiguration => contract.Properties.Any(x =>
                propertyConfiguration.Info.Name.Equals(x.UnderlyingName)) is false);

        foreach (var propertyConfiguration in propertiesToAdd)
        {
            var property = CreateProperty(propertyConfiguration.Info, MemberSerialization.OptOut);
            ResolveProperty(propertyConfiguration, property);

            contract.Properties.Add(property);
        }

        JsonProperty[] properties = contract.Properties
            .OrderBy(x => x.Order ?? -1)
            .ToArray();

        contract.Properties.Clear();

        foreach (var property in properties)
        {
            contract.Properties.Add(property);
        }

        foreach (var property in contract.CreatorParameters)
        {
            var propertyConfiguration = configuration.PropertyConfigurations
                .FirstOrDefault(x => property.PropertyName?.Equals(x.Info.Name) is true);

            if (propertyConfiguration is not null)
                ResolveProperty(propertyConfiguration, property);
        }

        var conversionConsumer = new ConversionConsumer();
        configuration.AcceptConversionConsumer(conversionConsumer);

        contract.Converter = conversionConsumer.Converter;

        return contract;
    }

    private void ResolveProperty(
        IPropertyConfiguration configuration,
        JsonProperty property)
    {
        var conversionConsumer = new ConversionConsumer();
        configuration.Accept(conversionConsumer);

        var serializationPredicateConsumer = new ObjectPredicateConsumer();
        configuration.AcceptSerializationFilterConsumer(serializationPredicateConsumer);

        var deserializationPredicateConsumer = new ObjectPredicateConsumer();
        configuration.AcceptDeserializationFilterConsumer(deserializationPredicateConsumer);

        var type = configuration.Info switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            _ => throw new ArgumentOutOfRangeException(nameof(configuration.Info)),
        };

        property.PropertyName = configuration.Name;
        property.UnderlyingName = configuration.Info.Name;
        property.Order = configuration.Position;
        property.DeclaringType = configuration.Info.DeclaringType;
        property.PropertyType = type;
        property.Converter = conversionConsumer.Converter;
        property.Ignored = configuration.AccessMode is ValueAccessMode.Ignore;
        property.Readable = configuration.AccessMode.HasFlag(ValueAccessMode.CanRead);
        property.Writable = configuration.AccessMode.HasFlag(ValueAccessMode.CanWrite);
        property.ShouldSerialize = serializationPredicateConsumer.Predicate;
        property.ShouldDeserialize = deserializationPredicateConsumer.Predicate;

        property.NullValueHandling = configuration.NullValueMode switch
        {
            NullValueMode.Include => NullValueHandling.Include,
            NullValueMode.Ignore => NullValueHandling.Ignore,
            _ => throw new ArgumentOutOfRangeException(),
        };

        if (configuration.SpecifyType is not null)
        {
            property.TypeNameHandling = configuration.SpecifyType.Value ? TypeNameHandling.All : TypeNameHandling.None;
        }
        else if (_configuration.ShouldEraseEnumerableType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            property.TypeNameHandling = TypeNameHandling.None;
        }
    }
}