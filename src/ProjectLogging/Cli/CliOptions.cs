
namespace ProjectLogging.Cli;



public static class CliOptions
{
    public static CliArgument Verbose => CliArgument.Create("verbose", "v", false, "Enable verbose output.", false);
}
