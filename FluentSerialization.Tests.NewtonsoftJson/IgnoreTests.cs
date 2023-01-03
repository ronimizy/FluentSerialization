using FluentAssertions;
using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class IgnoreTests
{
    private readonly JsonSerializerSettings _settings;

    public IgnoreTests()
    {
        _settings = new Configuration().Build().AsSettings();
    }

    [Fact]
    public async Task SerializeObject_Should_SerializeWithoutValueProperty()
    {
        // Arrange
        var obj = new Record(10, "sample");

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_SerializeWithoutValueProperty));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, _settings);

        // Assert
        await VerifyJson(serialized, verifySettings);
    }

    [Fact]
    public void DeserializeObject_Should_DeserializeWithoutValueProperty()
    {
        // Arrange
        var obj = new Record(10, "sample");
        var serialized = JsonConvert.SerializeObject(obj, _settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject<Record>(serialized, _settings);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Value.Should().Be(default);
        deserialized.Payload.Should().Be(obj.Payload);
    }

    public record Record(int Value, string Payload);

    public class Configuration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<Record>().Property(x => x.Value).Ignored();
        }
    }
}