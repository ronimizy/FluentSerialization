using FluentAssertions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class FieldsTest
{
    [Fact]
    public async Task SerializeObject_Should_SerializeFields()
    {
        // Arrange
        var obj = new Type();

        var settings = ConfigurationBuilder.Build(configuration =>
        {
            configuration.Type<Type>().Property("_a").Called("A");
            configuration.Type<Type>().Property(x => x._b).Called("B");
        }).AsNewtonsoftSerializationSettings();
        
        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_SerializeFields));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Assert
        await VerifyJson(serialized, verifySettings);
    }
    
    [Fact]
    public void DeserializeObject_Should_DeserializeFields()
    {
        // Arrange
        var obj = new Type();
        
        var settings = ConfigurationBuilder.Build(configuration =>
        {
            configuration.Type<Type>().Property("_a").Called("A");
            configuration.Type<Type>().Property(x => x._b).Called("B");
        }).AsNewtonsoftSerializationSettings();
        
        var serialized = JsonConvert.SerializeObject(obj, settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject<Type>(serialized, settings);

        // Assert
        deserialized.Should().BeEquivalentTo(obj);
    }
    
    public class Type
    {
        private int _a = 1;
        public int _b = 2;
    }
}