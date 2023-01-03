using FluentSerialization.Internal;

namespace FluentSerialization.Binding.Implementations;

/// <summary>
///     Helper type for binding a type configuration to builder, avoiding complicated generic reflection.
/// </summary>
internal class TypeConfigurationBinder<T> : ITypeConfigurationBinder
{
    private readonly ITypeSerializationConfiguration<T> _configuration;
    private readonly ITypeConfigurationBuilderInternal _configurationBuilder;

    public TypeConfigurationBinder(
        ITypeSerializationConfiguration<T> configuration,
        ITypeConfigurationBuilderInternal configurationBuilder)
    {
        _configuration = configuration;
        _configurationBuilder = configurationBuilder;
    }

    public void Bind()
    {
        ITypeConfigurationBuilder<T> builder = _configurationBuilder.Cast<T>();
        _configuration.Configure(builder);
    }
}