using FluentSerialization.Extensions;
using FluentSerialization.Extensions.NewtonsoftJson;
using Newtonsoft.Json;

namespace FluentSerialization.Tests.NewtonsoftJson;

[UsesVerify]
public sealed class IgnoreNullTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Serialize_ShouldHandleNullIgnoreCorrectly(bool ignoreNulls)
    {
        // Arrange
        JsonSerializerSettings serializationSettings = new Configuration(ignoreNulls)
            .Build()
            .AsNewtonsoftSerializationSettings();

        var model = new Model();

        var verifySettings = new VerifySettings();
        verifySettings.UseCurrentMethodName();
        verifySettings.UseParameters(ignoreNulls);

        // Act
        string serialized = JsonConvert.SerializeObject(model, serializationSettings);

        // Assert
        await Verify(serialized, verifySettings);
    }

    private sealed class Model
    {
        public Guid? NullableValue { get; set; }
    }

    public sealed class Configuration(bool ignoreNulls) : ISerializationConfiguration
    {
        public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Type<Model>(type =>
            {
                IPropertyConfigurationBuilder<Model, Guid?> property = type.Property(x => x.NullableValue);

                if (ignoreNulls)
                    property.IgnoreNulls();
            });
        }
    }
}
