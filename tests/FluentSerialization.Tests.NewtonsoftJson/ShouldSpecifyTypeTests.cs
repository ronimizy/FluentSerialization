using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class ShouldSpecifyTypeTests
{
    [Fact]
    public async Task SerializeObject_Should_SpecifyType()
    {
        // Arrange
        var obj = new Container(new SubRecord());

        var settings = SerializationConfigurationFactory.Build(configuration =>
        {
            configuration.Type<Container>().Property(x => x.Record).ShouldSpecifyType();
        }).AsNewtonsoftSerializationSettings();
        
        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_SpecifyType));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    [Fact]
    public async Task SerializeObject_Should_NotSpecifyType()
    {
        // Arrange
        var obj = new Container(new SubRecord());

        var settings = SerializationConfigurationFactory.Build(configuration =>
        {
            configuration.Type<Container>().Property(x => x.Record).ShouldSpecifyType(false);
        }).AsNewtonsoftSerializationSettings();
        
        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_NotSpecifyType));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    public record Record;
    
    public record SubRecord : Record;

    public record Container(Record Record);
}