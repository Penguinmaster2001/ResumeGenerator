
namespace ProjectLogging.Cli;



public interface ICliActionResult
{
    public static CliActionFailureResult Failure => new();
    public static CliActionFailureResult FailureMessage(string? message) => new(message);

    public static CliActionSuccessResult Success => new();
    public static CliActionSuccessResult SuccessMessage(string? message) => new(message);

    public static CliActionExceptionResult Exception(Exception exception) => new(exception);
}



public class CliActionFailureResult : ICliActionResult
{
    public string? Message { get; }



    public CliActionFailureResult(string? message = null)
    {
        Message = message;
    }
}



public class CliActionSuccessResult : ICliActionResult
{
    public string? Message { get; }



    public CliActionSuccessResult(string? message = null)
    {
        Message = message;
    }
}



public class CliActionExceptionResult : ICliActionResult
{
    public Exception Exception { get; }



    public CliActionExceptionResult(Exception exception)
    {
        Exception = exception;
    }
}
