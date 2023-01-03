namespace FluentSerialization.Exceptions;

public abstract class FluentSerializationException : Exception
{
    private protected FluentSerializationException(string message) : base(message) { }

    private protected FluentSerializationException(string message, Exception innerException)
        : base(message, innerException) { }
}