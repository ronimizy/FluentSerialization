# FluentSerialization [![badge](https://img.shields.io/nuget/v/ronimizy.FluentSerialization?style=flat-square)](https://www.nuget.org/packages/ronimizy.FluentSerialization/)

A package that provides a simple EF like fluent API for configuring serializer contracts.

Create a type, implementing `ISerializationConfiguration` like so:

```cs
public record A(int B, int C);

class SerializationConfiguration : ISerializationConfiguration
{
    public void Configure(ISerializationConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Type<A>(builder =>
        {
            builder.HasTypeKey("ATypeKey");
            builder.Property(a => a.B).Called("A");
            builder.Property(a => a.C).Ignored();
            builder.Property(x => x.B).ConvertedWith(x => (x + 2).ToString(), int.Parse);
        });
    }
}
```

To create an `IConfiguration` instance you can use `.Build` extensions method. \
You can also use a static `ConfigurationBuilder` class if you want to create an inline configuration, without creating a
separate class,
create an `IConfiguration` instance from multiple `ISerializationConfiguration`s or if you want to pass some
custom `IConfigurationValidator`s.

```cs
var configuration = ConfigurationBuilder.Build(configurationBuilder =>
{
    configurationBuilder.Type<A>(builder =>
    {
        builder.HasTypeKey("ATypeKey");
        builder.Property(a => a.B).Called("A");
        builder.Property(a => a.C).Ignored();
        builder.Property(x => x.B).ConvertedWith(x => (x + 2).ToString(), int.Parse);
    });
});
```

You can also add the configuration to service collection:

```cs
services.AddFluentSerialization(typeof(IAssemblyMarker));
services.AddFluentSerialization(new SerializationConfiguration());
```

## Configuration options

### Type

#### Type keys

Specifies a type key, used in metadata when serialized.

You can specify serialization type key:

```csharp
builder.HasSerializationTypeKey("ATypeKey");
```

You can specify deserialization type key:

```csharp
builder.HasDeserializationTypeKey("ATypeKey");
```

If you want to specify both, you can use `HasTypeKey`:

```csharp
builder.HasTypeKey("ATypeKey");
```

#### Type converters

Specifies a type converter, used when serializing/deserializing.

You can implement a `IConversion<TSource, TDestination>` interface and pass it to `ConvertedWith` method:

```csharp
builder.ConvertedWith(new AToBConverter());
```

Or you could also inline it passing a delegate:

```csharp
builder.ConvertedWith(x => new B(x.A), x => new A(x.B));
```

#### Type properties (or fields)

You can configure a public property or field using `Property` method:

```csharp
builder.Property(x => x.A);
```

If you want to configure non public property or field, you can use a `Property` method overload:

```csharp
builder.Property("_a");
```

### Property

#### Property name

You can configure a serialized property name using `Called` method:

```csharp
builder.Property(x => x.A).Called("a");
```

#### Ignoring

You can ignore a property using `Ignored` method:

```csharp
builder.Property(x => x.A).Ignored();
```

#### Property converters

Specifies a property converter, used when serializing/deserializing.

You can implement a `IConversion<TSource, TDestination>` interface and pass it to `ConvertedWith` method:

```csharp
builder.Property(x => x.A).ConvertedWith(new AToBConverter());
```

Or you could also inline it passing a delegate:

```csharp
builder.Property(x => x.A).ConvertedWith(x => new B(x.A), x => new A(x.B));
```

#### Conditional (de)serialization

You can specify a condition for serialization using `ShouldSerializeWhen` method:

```csharp
builder.Property(x => x.A).ShouldSerializeWhen(x => x.A > 0);
```

As well as for deserialization using `ShouldDeserializeWhen` method:

Note that the data that has been already deserialized will be passed to the predicate.
If you want to use some data to evaluate the predicate, make sure it will be deserialized before this property,
by placing it before this property in type definition or by using `PositionedAt`

```csharp
builder.Property(x => x.A).ShouldDeserializeWhen(x => x.A > 0);
```

#### Property position

You can specify a position of a property in serialized data using `PositionedAt` method:

```csharp
builder.Property(x => x.A).PositionedAt(2);
builder.Property(x => x.B).PositionedAt(1);
```

# FluentSerialization.Extensions.NewtonsoftJson [![badge](https://img.shields.io/nuget/v/ronimizy.FluentSerialization.Extensions.NewtonsoftJson?style=flat-square)](https://www.nuget.org/packages/ronimizy.FluentSerialization.Extensions.NewtonsoftJson/)

A provider for configuring Newtonsoft.Json contract resolvers / serializer settings.

To convert use `.AsNewtonsoftSerializationSettings` extension method:

```cs
var configuration = ConfigurationBuilder.Build(configurationBuilder =>
{
    configurationBuilder.Type<A>(builder =>
    {
        builder.HasTypeKey("ATypeKey");
        builder.Property(a => a.B).Called("A");
        builder.Property(a => a.C).Ignored();
        builder.Property(x => x.B).ConvertedWith(x => (x + 2).ToString(), int.Parse);
    });
});

var settings = configuration.AsNewtonsoftSerializationSettings();
```

You can add `JsonSerializerSettings` to service collection:

```cs
services.AddFluentSerialization(typeof(IAssemblyMarker)).AddNewtonsoftJson();
```
