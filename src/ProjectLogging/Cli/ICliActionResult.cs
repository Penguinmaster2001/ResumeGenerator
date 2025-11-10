
namespace ProjectLogging.Cli;



public interface ICliActionResult
{

}



public class CliActionFailureResult : ICliActionResult
{
    public string Message { get; }



    public CliActionFailureResult(string message)
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
