namespace FluentSerialization;

/// <summary>
///     A tree representation type hierarchy presented in configuration
/// </summary>
public interface ITypeHierarchyConfiguration : ITypeConfiguration
{
    IReadOnlyCollection<ITypeConfiguration> Children { get; }
}