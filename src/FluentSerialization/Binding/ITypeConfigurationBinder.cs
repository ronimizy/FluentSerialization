namespace FluentSerialization.Binding;

/// <summary>
///     Helper type for binding a type configuration to builder, avoiding complicated generic reflection.
/// </summary>
internal interface ITypeConfigurationBinder
{
    void Bind();
}