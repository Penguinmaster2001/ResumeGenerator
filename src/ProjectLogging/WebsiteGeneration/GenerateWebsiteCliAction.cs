
using ProjectLogging.Cli;
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration;



namespace ProjectLogging.WebsiteGeneration;



public static class GenerateWebsiteCliAction
{
    public static CliAction CliAction => new("generate", "website", "Generate a website", GenerateWebsiteAsync, new([
        CliArgument.Create<string>("output", "o", true, "Output path.", null),
        CliArgument.Create<string>("projects", "p", true, "Project readmes.", null),
    ]));




    public static async Task<ICliActionResult> GenerateWebsiteAsync(CliParseResults parsedCli)
    {
        var outDir = parsedCli.Arguments.GetArgument<string>("output");
        var projectJson = parsedCli.Arguments.GetArgument<string>("projects");

        var projects = await RecordLoader.LoadProjectReadmeAsync(projectJson);

        var website = WebsiteGenerator.GenerateWebsite(outDir, projects);

        await website.CreateFilesAsync();

        return new GenerateResumeResult();
    }
}
