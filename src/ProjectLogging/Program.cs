
using ProjectLogging.Cli;
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.WebsiteGeneration;



namespace ProjectLogging;



public static class Program
{
    public static async Task<int> Main()
    {
        var cliParser = new CliParser(
            [
                GenerateResumeCliAction.CliAction,
                GenerateWebsiteCliAction.CliAction,
                TestParseReadmeCliAction.CliAction,
            ],
            [
                CliOptions.Verbose,
            ]);

        var parseResults = cliParser.ParseArgs(Environment.GetCommandLineArgs());

        var result = await parseResults.Action.Action.Invoke(parseResults);

        switch (result)
        {
            case CliActionFailureResult failure:
                if (failure.Message is not null)
                {
                    Console.WriteLine($"Failure: {failure.Message}");
                }
                return -1;

            case CliActionSuccessResult success:
                if (success.Message is not null)
                {
                    Console.WriteLine($"Success: {success.Message}");
                }
                return 0;

            case GenerateResumeResult generateResumeResult:
                Console.WriteLine("Finished generating resume.");
                return 0;
        }

        return 0;
    }
}
