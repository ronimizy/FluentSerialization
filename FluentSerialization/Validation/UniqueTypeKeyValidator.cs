using System.Text;
using FluentSerialization.Models;

namespace FluentSerialization;

/// <summary>
///     Validator for unique type keys
/// </summary>
internal class UniqueTypeKeyValidator : IConfigurationValidator
{
    public ConfigurationValidationResult Validate(IReadOnlyCollection<ITypeConfiguration> configurations)
    {
        IEnumerable<DuplicateInfo> serializationDuplicated = GetDuplicates(
            configurations, "serialization", c => c.SerializationKey);

        IEnumerable<DuplicateInfo> deserializationDuplicated = GetDuplicates(
            configurations, "deserialization", c => c.DeserializationKey);

        IEnumerable<DuplicateInfo> duplicates = serializationDuplicated.Concat(deserializationDuplicated);

        var builder = new StringBuilder(0);

        foreach ((var kind, var key, ITypeConfiguration[] types, var count) in duplicates)
        {
            builder.AppendLine($"Duplicate {kind} type key '{key}' found for {count} types:");
            foreach (var type in types)
            {
                builder.AppendLine($"\t{type.Type}");
            }
        }

        var message = builder.ToString();

        return message.Length is 0
            ? ConfigurationValidationResult.Success()
            : ConfigurationValidationResult.Failure(message);
    }

    private static IEnumerable<DuplicateInfo> GetDuplicates(
        IEnumerable<ITypeConfiguration> configurations,
        string kind,
        Func<ITypeConfiguration, string> selector)
    {
        return configurations
            .GroupBy(selector)
            .Select(x => (key: x.Key, types: x.ToArray()))
            .Select(x => new DuplicateInfo(kind, x.key, x.types, Count: x.types.Length))
            .Where(x => x.Count > 1);
    }

    private readonly record struct DuplicateInfo(string Kind, string Key, ITypeConfiguration[] Types, int Count);
}