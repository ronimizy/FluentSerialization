using FluentAssertions;
using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class CalledTests
{
    private readonly JsonSerializerSettings _settings;

    public CalledTests()
    {
        _settings = new Configuration().Build().AsSettings();
    }

    [Fact]
    public async Task SerializeObject_Should_SerializeObjectWithCustomPropertyName()
    {
        // Arrange
        var obj = new Record(123, "abc");

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_SerializeObjectWithCustomPropertyName));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, _settings);

        // Assert
        await VerifyJson(serialized, verifySettings);
    }

    [Fact]
    public void DeserializeObject_Should_DeserializeObjectWithCustomPropertyName()
    {
        // Arrange
        var obj = new Record(123, "abc");
        var serialized = JsonConvert.SerializeObject(obj, _settings);

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(DeserializeObject_Should_DeserializeObjectWithCustomPropertyName));

        // Act
        var deserialize = JsonConvert.DeserializeObject<Record>(serialized, _settings);

        // Assert
        deserialize.Should().NotBeNull();
        deserialize.Value.Should().Be(obj.Value);
        deserialize.Payload.Should().Be(obj.Payload);
    }

    public record Record(int Value, string Payload);

    public class Configuration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<Record>().Property(x => x.Value).Called("Integer");
        }
    }
}