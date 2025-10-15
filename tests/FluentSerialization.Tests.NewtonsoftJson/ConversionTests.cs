using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public sealed class ConversionTests
{
    private readonly JsonSerializerSettings _serializerSettings;

    public ConversionTests()
    {
        _serializerSettings = new Configuration().Build().AsNewtonsoftSerializationSettings();
    }

    [Fact]
    public async Task Deserialize_ShouldDeserializeValue_WhenSerializedWithoutConverterAndSerializedWithConverter()
    {
        // Arrange
        var userId = new UserId(Guid.Empty);

        var verifySettings = new VerifySettings();
        verifySettings.UseMethodName(nameof(Deserialize_ShouldDeserializeValue_WhenSerializedWithoutConverterAndSerializedWithConverter));

        // Act
        string serialized = JsonConvert.SerializeObject(userId, _serializerSettings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    public sealed record UserId(Guid Value);

    public sealed class UserIdConversion : IConversion<UserId, Guid>
    {
        public Guid ConvertTo(UserId source) => source.Value;

        public UserId ConvertFrom(Guid destination) => new(destination);
    }

    private sealed class Configuration : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conversion(new UserIdConversion());
        }
    }
}
