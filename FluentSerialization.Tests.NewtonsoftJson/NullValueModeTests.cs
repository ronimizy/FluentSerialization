using FluentSerialization.Extensions.NewtonsoftJson;
using FluentSerialization.Models;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class NullValueModeTests
{
    [Fact]
    public async Task SerializeObject_ShouldIgnoreNullValues_WhenConfigured()
    {
        // Arrange
        var obj = new Record(null);

        var settings = ConfigurationBuilder
            .Build(configuration => configuration
                .Type<Record>().Property(x => x.Value).WithNullValueMode(NullValueMode.Ignore))
            .AsNewtonsoftSerializationSettings();

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_ShouldIgnoreNullValues_WhenConfigured));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }
    
    [Fact]
    public async Task SerializeObject_ShouldWriteNullValues_WhenConfigured()
    {
        // Arrange
        var obj = new Record(null);

        var settings = ConfigurationBuilder
            .Build(configuration => configuration
                .Type<Record>().Property(x => x.Value).WithNullValueMode(NullValueMode.Include))
            .AsNewtonsoftSerializationSettings();

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_ShouldWriteNullValues_WhenConfigured));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    public record Record(string? Value);
}