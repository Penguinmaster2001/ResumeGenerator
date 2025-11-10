
using ProjectLogging.Cli;
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.WebsiteGeneration;



namespace ProjectLogging;



public static class Program
{
    public static async Task<int> Main()
    {
        var cliParser = new CliParser(
            [GenerateResumeCliAction.CliAction],
            []
        );

        var parseResults = cliParser.ParseArgs(Environment.GetCommandLineArgs());

        var result = await parseResults.Action.Action.Invoke(parseResults.Arguments);

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



    public static void GenerateWebsite(ResumeModel resumeModel, string outDir)
    {
        WebsiteGenerator.GenerateWebsite(resumeModel, outDir).CreateFiles();
    }
}
