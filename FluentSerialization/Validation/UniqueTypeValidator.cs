using System.Text;
using FluentSerialization.Models;

namespace FluentSerialization;

/// <summary>
///     Validator for unique type configuration
/// </summary>
internal class UniqueTypeValidator : IConfigurationValidator
{
    public ConfigurationValidationResult Validate(IReadOnlyCollection<ITypeConfiguration> configurations)
    {
        IEnumerable<(Type key, int count)> types = configurations
            .GroupBy(x => x.Type)
            .Select(x => (key: x.Key, count: x.Count()))
            .Where(x => x.count > 1);

        var builder = new StringBuilder(0);

        foreach (var (type, count) in types)
        {
            builder.AppendLine($"Type {type} is configured {count} times.");
        }

        var message = builder.ToString();

        return message.Length is 0
            ? ConfigurationValidationResult.Success()
            : ConfigurationValidationResult.Failure(message);
    }
}