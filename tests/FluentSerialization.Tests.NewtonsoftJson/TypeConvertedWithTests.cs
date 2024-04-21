using FluentAssertions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class TypeConvertedWithTests
{
    [Fact]
    public async Task SerializeObject_Should_UseTypeConverterCorrectly()
    {
        // Arrange
        var obj = new Record("test");

        var settings = SerializationConfigurationFactory.Build(configuration =>
        {
            configuration.Type<Record>().ConvertedWith(x => x.Value, x => new Record(x));
        }).AsNewtonsoftSerializationSettings();

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_UseTypeConverterCorrectly));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    [Fact]
    public void DeserializeObject_Should_UseTypeConverterCorrectly()
    {
        // Arrange
        var settings = SerializationConfigurationFactory.Build(configuration =>
        {
            configuration.Type<Record>().ConvertedWith(x => x.Value, x => new Record(x));
        }).AsNewtonsoftSerializationSettings();

        var obj = new Record("test");
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject<Record>(serialized, settings);

        // Assert
        deserialized.Should().BeEquivalentTo(obj);
    }

    public record Record(string Value);
}