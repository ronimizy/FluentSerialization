using FluentAssertions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class PredicateTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    public async Task SerializeObject_Should_SerializeWhenValueIsOdd(int value)
    {
        // Arrange
        var settings = ConfigurationBuilder.Build(builder =>
        {
            builder.Type<Record>().Property(x => x.Value).ShouldSerializeWhen(x => x.Value % 2 is 1);
        }).AsSettings();

        var obj = new Record(value, "one");

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_SerializeWhenValueIsOdd));
        verifySettings.UseParameters(value);

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await VerifyJson(serialized, verifySettings);
    }

    [Theory]
    [InlineData(true, 20, 20)]
    [InlineData(false, 20, 0)]
    public void DeserializeObject_Should_DeserializeWhenFlagInOn(bool flag, int value ,int expected)
    {
        // Arrange
        var settings = ConfigurationBuilder.Build(builder =>
        {
            builder.Type<PlainObject>().Property(x => x.Value).ShouldDeserializeWhen(x => x.ShouldDeserialize);
        }).AsSettings();

        var obj = new PlainObject { ShouldDeserialize = flag, Value = value };
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject<PlainObject>(serialized, settings);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Value.Should().Be(expected);
    }

    public record Record(int Value, string Payload);

    public class PlainObject
    {
        public bool ShouldDeserialize { get; set; }
        public int Value { get; set; }
    }
}