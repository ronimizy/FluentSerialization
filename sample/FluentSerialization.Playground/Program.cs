// See https://aka.ms/new-console-template for more information

using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using FluentSerialization.Playground;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

var collection = new ServiceCollection();

collection.AddFluentSerialization(c => c.WithConfiguration<SerializationConfiguration>());

collection
    .AddOptions<JsonSerializerSettings>()
    .UseFluentSerialization();

collection.AddSingleton(p => p.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

var provider = collection.BuildServiceProvider();
var settings = provider.GetRequiredService<JsonSerializerSettings>();

IReadOnlyCollection<A> array = new[]
{
    new A(1, 2),
    new D(3, 4),
};

var x = new X(array);

var serializedX = JsonConvert.SerializeObject(x, settings);

var a = new A(1, 2);

Console.WriteLine(a);

var str = JsonConvert.SerializeObject(a, typeof(object), settings);
Console.WriteLine(str);

var b = JsonConvert.DeserializeObject(str, settings);
Console.WriteLine(b);

Console.WriteLine();

namespace FluentSerialization.Playground
{
    public record A(int B, int C);

    public record D(int B, int C) : A(B, C);

    public record X(IReadOnlyCollection<A> Array);

    class SerializationConfiguration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<A>(builder =>
            {
                builder.HasTypeKey("Aboba");
                builder.Property(a => a.B).Called("A");
                builder.Property(a => a.C).Ignored();
                builder.Property(x => x.B).ConvertedWith(x => (x + 2).ToString(), int.Parse);
            });

            configurationBuilder.Type<D>().HasTypeKey("D");
        }
    }
}