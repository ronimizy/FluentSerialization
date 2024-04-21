using FluentAssertions;
using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public class TypeKeyTests
{
    [Fact]
    public async Task SerializeObject_Should_UseCorrectTypeKey()
    {
        // Arrange
        var obj = new FirstDerived(1, "a");
        var settings = new Configuration().Build().AsNewtonsoftSerializationSettings();

        settings = new JsonSerializerSettings(settings)
        {
            Formatting = Formatting.Indented,
        };

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_UseCorrectTypeKey));

        // Act
        var serialized = JsonConvert.SerializeObject(obj, typeof(Base), settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    [Fact]
    public async Task SerializeObject_Should_UseCorrectTypesOnArray()
    {
        // Arrange
        var array = new Base[]
        {
            new FirstDerived(1, "a"),
            new SecondDerived(2, 1.2),
        };

        var settings = new Configuration().Build().AsNewtonsoftSerializationSettings();

        settings = new JsonSerializerSettings(settings)
        {
            Formatting = Formatting.Indented,
        };

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(SerializeObject_Should_UseCorrectTypesOnArray));

        // Act
        var serialized = JsonConvert.SerializeObject(array, settings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    [Fact]
    public void DeserializeObject_Should_DeserializeWithCorrectType()
    {
        // Arrange
        var obj = new FirstDerived(1, "a");
        var settings = new Configuration().Build().AsNewtonsoftSerializationSettings();
        var serialized = JsonConvert.SerializeObject(obj, typeof(Base), settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject(serialized, typeof(Base), settings);

        // Assert
        deserialized.Should().BeEquivalentTo(obj);
    }

    [Fact]
    public void DeserializeObject_Should_DeserializeWithCorrectTypesOnArray()
    {
        // Arrange
        var array = new Base[]
        {
            new FirstDerived(1, "a"),
            new SecondDerived(2, 1.2),
        };
        var settings = new Configuration().Build().AsNewtonsoftSerializationSettings();
        var serialized = JsonConvert.SerializeObject(array, settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject<Base[]>(serialized, settings);

        // Assert
        deserialized.Should().BeEquivalentTo(array);
    }

    [Fact]
    public void ObjectTypeTransfer_Should_WorkCorrectlyWithTypeKeys()
    {
        // Arrange
        var obj = new FirstDerived(1, "a");

        var settings = SerializationConfigurationFactory.Build(configuration =>
        {
            configuration.Type<FirstDerived>().HasSerializationTypeKey("A");
            configuration.Type<ThirdDerived>().HasDeserializationTypeKey("A");
        }).AsNewtonsoftSerializationSettings();

        var serialized = JsonConvert.SerializeObject(obj, typeof(Base), settings);

        // Act
        var deserialized = JsonConvert.DeserializeObject(serialized, typeof(Base), settings);

        // Assert
        var third = deserialized.Should().BeOfType<ThirdDerived>().Subject;
        third.Should().BeEquivalentTo(obj);
    }

    public abstract record Base(int Value);

    public record FirstDerived(int Value, string Payload) : Base(Value);

    public record SecondDerived(int Value, double Size) : Base(Value);

    public record ThirdDerived(int Value, string Payload) : Base(Value);

    public class Configuration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<Base>().HasTypeKey("BaseKey");
            configurationBuilder.Type<FirstDerived>().HasTypeKey("FirstDerivedKey");
            configurationBuilder.Type<SecondDerived>().HasTypeKey("SecondDerivedKey");
        }
    }
}