
using ProjectLogging.Cli;



namespace ProjectLogging.Projects;



public static class TestParseReadmeCliAction
{
    public static CliAction CliAction => new("test", "readme-parser", "Print out parsed readme", TestParseReadmeAsync, new([
        CliArgument.Create<string>("path", "p", true, "Readme path", null),
    ]));




    public static async Task<ICliActionResult> TestParseReadmeAsync(CliParseResults parsedCli)
    {
        var input = parsedCli.Arguments.GetArgument<string>("path");

        var result = await ProjectReadmeParser.ParseReadmeAsync(input);

        if (result.Successful)
        {
            Console.WriteLine(result);

            return ICliActionResult.Success;
        }

        return ICliActionResult.Failure;
    }
}
