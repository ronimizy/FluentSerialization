using System.Text;
using FluentSerialization.Exceptions;
using FluentSerialization.Implementations;
using FluentSerialization.Models;
using FluentSerialization.Tools;

namespace FluentSerialization;

/// <summary>
///     Helper class for configuration building 
/// </summary>
public static class SerializationConfigurationFactory
{
    private static readonly IConfigurationValidator[] Validators =
    {
        new UniqueTypeKeyValidator(),
        new UniqueTypeValidator(),
    };

    /// <summary>
    ///     Builds a configuration from given delegate
    /// </summary>
    public static IConfiguration Build(Action<ISerializationConfigurationBuilder> action)
    {
        var configuration = new DelegateSerializationConfiguration(action);
        return Build(configuration);
    }

    /// <summary>
    ///     Builds a configuration from given serialization configurations
    /// </summary>
    public static IConfiguration Build(params ISerializationConfiguration[] configurations)
        => Build(configurations, Array.Empty<IConfigurationValidator>());

    /// <summary>
    ///     Builds a configuration from given serialization configurations and validators
    /// </summary>
    /// <exception cref="SerializationConfigurationException">Configuration is invalid</exception>
    public static IConfiguration Build(
        IEnumerable<ISerializationConfiguration> configurations,
        IEnumerable<IConfigurationValidator> validators)
    {
        var options = new FluentSerializationOptions();

        ITypeConfiguration[] typeConfigurations = configurations
            .Select(x =>
            {
                var serializationBuilder = new SerializationConfigurationBuilder(options);
                x.Configure(serializationBuilder);

                return serializationBuilder;
            })
            .SelectMany(x => x.Builders)
            .Select(x => x.Build())
            .ToArray();

        ConfigurationValidationResult.FailureResult[] errors = Validators.Concat(validators)
            .Select(x => x.Validate(typeConfigurations))
            .OfType<ConfigurationValidationResult.FailureResult>()
            .ToArray();

        if (errors.Length is 0)
            return new Configuration(BuildForest(typeConfigurations), options);

        var builder = new StringBuilder("Invalid serialization configuration:\n");
        Exception? exception = null;

        foreach (var error in errors)
        {
            builder.AppendLine(error.Reason);

            if (error.Exception is null)
                continue;

            exception = exception is null
                ? error.Exception
                : new AggregateException(exception, error.Exception);
        }

        var message = builder.ToString();

        throw SerializationConfigurationException.InvalidConfiguration(message, exception);
    }

    /// <summary>
    ///     Builds a forest for hierarchical type configurations
    /// </summary>
    private static IReadOnlyCollection<ITypeConfiguration> BuildForest(IReadOnlyCollection<ITypeConfiguration> pool)
        => pool.Select(x => BuildRoot(x, pool)).ToArray();

    private static ITypeConfiguration BuildRoot(ITypeConfiguration root, IEnumerable<ITypeConfiguration> pool)
    {
        ITypeConfiguration[] assignable = pool
            .Where(x => x.Type != root.Type)
            .Where(x => root.Type.IsAssignableFrom(x.Type))
            .ToArray();

        IReadOnlyCollection<ITypeConfiguration> children = BuildForest(assignable);

        return children.Count is 0 ? root : new TypeHierarchyConfigurationProxy(root, children);
    }
}