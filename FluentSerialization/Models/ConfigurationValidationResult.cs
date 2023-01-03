namespace FluentSerialization.Models;

public class ConfigurationValidationResult
{
    private ConfigurationValidationResult() { }

    public static ConfigurationValidationResult Success()
        => new SuccessResult();

    public static ConfigurationValidationResult Failure(string message)
        => new FailureResult(message);

    public static ConfigurationValidationResult Failure(string message, Exception exception)
        => new FailureResult(message, exception);

    public static ConfigurationValidationResult Failure(Exception exception)
        => new FailureResult(string.Empty, exception);

    public static ConfigurationValidationResult Failure()
        => new FailureResult(string.Empty);

    internal class SuccessResult : ConfigurationValidationResult { }

    internal class FailureResult : ConfigurationValidationResult
    {
        public FailureResult(string reason, Exception? exception = null)
        {
            Reason = reason;
            Exception = exception;
        }

        public string Reason { get; }

        public Exception? Exception { get; }
    }
}