namespace FluentSerialization.Internal;

/// <summary>
///     Internal interface for storing generic property builders
/// </summary>
internal interface IPropertyConfigurationBuilderInternal<out THost>
{
    IPropertyConfigurationBuilder<THost, TProperty> Cast<TProperty>();

    IPropertyConfigurationBuilder<THost, object> EraseType();

    IPropertyConfiguration Build();
}

internal interface IPropertyConfigurationBuilderInternal<out THost, TProperty> :
    IPropertyConfigurationBuilder<THost, TProperty>,
    IPropertyConfigurationBuilderInternal<THost> { }