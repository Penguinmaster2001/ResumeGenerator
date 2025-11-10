
using ProjectLogging.Cli;
using ProjectLogging.ResumeGeneration;



namespace ProjectLogging;



public static class Program
{
    public static async Task<int> Main()
    {
        var cliParser = new CliParser(
            [GenerateResumeCliAction.CliAction],
            [CliOptions.Verbose]
        );

        var parseResults = cliParser.ParseArgs(Environment.GetCommandLineArgs());

        var result = await parseResults.Action.Action.Invoke(parseResults);

        switch (result)
        {
            case CliActionFailureResult cliActionFailureResult:
                Console.WriteLine($"Error: {cliActionFailureResult.Message}");
                return -1;
            case GenerateResumeResult generateResumeResult:
                Console.WriteLine("Finished generating resume.");
                return 0;
        }

        return 0;
    }
}
