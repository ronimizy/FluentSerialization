namespace FluentSerialization;

/// <summary>
///     Visitor for hierarchical type configurations.
/// </summary>
public interface ITypeConfigurationVisitor
{
    void Visit(ITypeConfiguration configuration);

    void Visit(ITypeHierarchyConfiguration configuration);
}