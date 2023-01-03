using FluentAssertions;
using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class PropertyConvertedWithTests
{
    private readonly JsonSerializerSettings _settings;

    public PropertyConvertedWithTests()
    {
        _settings = new Configuration().Build().AsNewtonsoftSerializationSettings();
    }

    [Fact]
    public async Task SerializeObject_Should_ConvertValuesWhenSerialized()
    {
        // Arrange
        var obj = new Record(123, "abc");
        
        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_ConvertValuesWhenSerialized));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, _settings);

        // Assert
        await VerifyJson(serialized, verifySettings);
    }
    
    [Fact]
    public void DeserializeObject_Should_ConvertValuesWhenDeserialized()
    {
        // Arrange
        var obj = new Record(123, "abc");
        var serialized = JsonConvert.SerializeObject(obj, _settings);
        
        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(DeserializeObject_Should_ConvertValuesWhenDeserialized));

        // Act
        var deserialized = JsonConvert.DeserializeObject<Record>(serialized, _settings);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Value.Should().Be(obj.Value);
        deserialized.Payload.Should().Be(obj.Payload);
    }

    public record Record(int Value, string Payload);

    public class Configuration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<Record>(builder =>
            {
                builder.Property(x => x.Value).ConvertedWith(ConvertTo, ConvertFrom);
                builder.Property(x => x.Payload).ConvertedWith(new Conversion());
            });
        }
    }
    
    public class Conversion : IConversion<string, object>
    {
        public object ConvertTo(string source)
        {
            return new { Value = source };
        }

        public string ConvertFrom(object destination)
        {
            dynamic d = destination;
            return d.Value;
        }
    }

    private static string ConvertTo(int value)
        => (value + 2).ToString();
    
    public static int ConvertFrom(string value)
        => int.Parse(value) - 2;
}