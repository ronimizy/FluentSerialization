namespace FluentSerialization.Exceptions;

public class SerializationConfigurationException : FluentSerializationException
{
    internal SerializationConfigurationException(string message) : base(message) { }

    internal SerializationConfigurationException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    ///     Configuration validators failed.
    /// </summary>
    internal static SerializationConfigurationException InvalidConfiguration(string message, Exception? innerException)
    {
        return innerException is null
            ? new SerializationConfigurationException(message)
            : new SerializationConfigurationException(message, innerException);
    }
}