
using ProjectLogging.Cli;
using ProjectLogging.ResumeGeneration;



namespace ProjectLogging.WebsiteGeneration;



public static class GenerateWebsiteCliAction
{
    public static CliAction CliAction => new("generate", "website", "Generate a website", GenerateWebsiteAsync, new([
        CliArgument.Create<string>("output", "o", true, "Output path.", null),
    ]));




    public static async Task<ICliActionResult> GenerateWebsiteAsync(CliParseResults parsedCli)
    {
        var outDir = parsedCli.Arguments.GetArgument<string>("output");

        var website = WebsiteGenerator.GenerateWebsite(outDir);

        await website.CreateFilesAsync();
        
        return new GenerateResumeResult();
    }
}
