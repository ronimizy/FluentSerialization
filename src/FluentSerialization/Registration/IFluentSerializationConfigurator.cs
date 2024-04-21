namespace FluentSerialization.Registration;

public interface IFluentSerializationConfigurator
{
    IFluentSerializationConfigurator WithConfiguration<TConfiguration>()
        where TConfiguration : class, ISerializationConfiguration;

    IFluentSerializationConfigurator WithValidator<TValidator>()
        where TValidator : class, IConfigurationValidator;
}