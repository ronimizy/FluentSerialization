namespace FluentSerialization.Models;

/// <summary>
///     Value access that serialized will have
/// </summary>
[Flags]
public enum ValueAccessMode
{
    /// <summary>
    ///     Value can be read from the source.
    /// </summary>
    CanRead = 1 << 1,

    /// <summary>
    ///     Value can be written to the source.
    /// </summary>
    CanWrite = 1 << 2,

    /// <summary>
    ///     Value can be read from and written to the source.
    /// </summary>
    CanReadAndWrite = CanRead | CanWrite,

    /// <summary>
    ///     Value is ignored.
    /// </summary>
    Ignore = 1 << 3,
}