using FluentSerialization.Models;

namespace FluentSerialization;

public interface IConfigurationValidator
{
    ConfigurationValidationResult Validate(IReadOnlyCollection<ITypeConfiguration> configurations);
}