namespace FluentSerialization.Internal;

/// <summary>
///     Internal interface for storing generic type builders
/// </summary>
internal interface ITypeConfigurationBuilderInternal
{
    ITypeConfigurationBuilder<T> Cast<T>();

    ITypeConfiguration Build();
}