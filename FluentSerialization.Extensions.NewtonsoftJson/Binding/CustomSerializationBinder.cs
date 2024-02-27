using Newtonsoft.Json.Serialization;

namespace FluentSerialization.Extensions.NewtonsoftJson.Binding;

internal class CustomSerializationBinder : DefaultSerializationBinder
{
    private readonly IConfiguration _configuration;
    private readonly ISerializationBinder? _binder;

    public CustomSerializationBinder(IConfiguration configuration, ISerializationBinder? binder)
    {
        _configuration = configuration;
        _binder = binder;
    }

    public override Type BindToType(string? assemblyName, string typeName)
    {
        var visitor = new DeserializationKeyLocatingVisitor(typeName);

        foreach (var configuration in _configuration.Types)
        {
            configuration.Accept(visitor);

            if (visitor.Type is not null)
                return visitor.Type;
        }

        return visitor.Type ?? _binder?.BindToType(assemblyName, typeName) ?? base.BindToType(assemblyName, typeName);
    }

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        var visitor = new SerializationKeyLocatingVisitor(serializedType);
        assemblyName = null;

        foreach (var configuration in _configuration.Types)
        {
            configuration.Accept(visitor);

            if (visitor.Key is null)
                continue;

            typeName = visitor.Key;
            return;
        }

        if (_binder is not null)
        {
            _binder.BindToName(serializedType, out assemblyName, out typeName);
            return;
        }

        base.BindToName(serializedType, out assemblyName, out typeName);
    }
}