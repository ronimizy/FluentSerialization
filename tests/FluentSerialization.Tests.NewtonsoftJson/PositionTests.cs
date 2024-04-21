using FluentAssertions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class PositionTests
{
    [Fact]
    public async Task SerializeObject_Should_UseSpecifiedPositions()
    {
        // Arrange
        var obj = new Record(1, 2);

        var settings = ConfigurationBuilder.Build(configuration =>
        {
            configuration.Type<Record>(builder =>
            {
                builder.Property(x => x.A).PositionedAt(2);
                builder.Property(x => x.B).PositionedAt(1);
            });
        }).AsNewtonsoftSerializationSettings();

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_UseSpecifiedPositions));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    [Fact]
    public void DeserializeObject_Should_DeserializeValuesCorrectlyWhenPositionsSpecified()
    {
        // Arrange
        var obj = new Record(1, 2);

        var settings = ConfigurationBuilder.Build(configuration =>
        {
            configuration.Type<Record>(builder =>
            {
                builder.Property(x => x.A).PositionedAt(2);
                builder.Property(x => x.B).PositionedAt(1);
            });
        }).AsNewtonsoftSerializationSettings();
        
        var serialized = JsonConvert.SerializeObject(obj, settings);
        
        // Act
        var deserialized = JsonConvert.DeserializeObject<Record>(serialized, settings);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.A.Should().Be(obj.A);
        deserialized.B.Should().Be(obj.B);
    }

    public record Record(int A, int B);
}