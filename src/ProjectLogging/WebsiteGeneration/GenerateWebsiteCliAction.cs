
using System.Text.Json;
using ProjectLogging.Cli;
using ProjectLogging.Data;
using ProjectLogging.Models.Website;
using ProjectLogging.Projects;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public static class GenerateWebsiteCliAction
{
    public static CliAction CliAction => new("generate", "website", "Generate a website", GenerateWebsiteAsync, new([
        CliArgument.Create<string>("output", "o", true, "Output path.", null),
        CliArgument.Create<string>("projects", "p", true, "Project readmes.", null),
        CliArgument.Create<string>("settings", "s", true, "Settings file.", null),
        CliArgument.Create("strict", "r", false, "Output path.", false),
    ]));



    public static async Task<ICliActionResult> GenerateWebsiteAsync(CliParseResults parsedCli)
    {
        var outDir = parsedCli.Arguments.GetArgument<string>("output");
        var projectJson = parsedCli.Arguments.GetArgument<string>("projects");
        var settingsPath = parsedCli.Arguments.GetArgument<string>("settings");

        var settings = await JsonSerializer.DeserializeAsync<WebsiteGenerationSettings>(File.OpenRead(settingsPath));

        if (settings is null)
        {
            return new CliActionFailureResult("Unable to read settings.");
        }

        var projects = await RecordLoader.LoadProjectReadmeAsync(projectJson);

        var dataConfig = JsonSerializer.Deserialize<DataConfig>(File.OpenRead(settings.DataConfigPath));

        if (dataConfig is null)
        {
            return new CliActionFailureResult("Unable to read data config.");
        }

        var baseDataPath = settings.DataConfigPath[0..(settings.DataConfigPath.Length-Path.GetFileName(settings.DataConfigPath).Length)];
        var personalInfo = RecordLoader.LoadPersonalInfoAsync(Path.Combine(baseDataPath, dataConfig.PersonalInfo.Path));
        var entries = RecordLoader.LoadEntriesAsync<Dictionary<string, object>>(baseDataPath, dataConfig.Other);

        await Task.WhenAll(personalInfo, entries);

        var dataCollection = new DataCollection(dataConfig);
        dataCollection.AddData(dataConfig.PersonalInfo.Title, personalInfo.Result);
        dataCollection.AddData(entries.Result);

        var website = await WebsiteGenerator.GenerateWebsiteAsync(outDir, dataCollection, settings!, projects);

        await website.CreateFilesAsync();

        return ICliActionResult.Success;
    }



    public static CliAction TestHtmlTemplateCliAction => new("test", "html-template", "Test the html template class.",
        TestHtmlTemplateAsync, new([
        CliArgument.Create<string>("template", "t", true, "Input template.", null),
        CliArgument.Create<string>("projects", "p", true, "Project readmes.", null),
        CliArgument.Create<string>("test-project", "r", true, "Project to test.", null),
        CliArgument.Create<string>("output", "o", false, "Output path.", null),
        CliArgument.Create("strict", "s", false, "Output path.", false),
    ]));



    public static async Task<ICliActionResult> TestHtmlTemplateAsync(CliParseResults parsedCli)
    {
        var templatePath = parsedCli.Arguments.GetArgument<string>("template");
        var projectJson = parsedCli.Arguments.GetArgument<string>("projects");
        var testProjectTitle = parsedCli.Arguments.GetArgument<string>("test-project");

        var projects = await RecordLoader.LoadProjectReadmeAsync(projectJson);
        var project = projects.Find(p => string.Equals(p.Title, testProjectTitle, StringComparison.OrdinalIgnoreCase));

        var card = new ProjectCard(project!);
        var info = await ProjectInfo.CreateFromCardAsync(card);

        Console.WriteLine(info.Features);

        var htmlTemplate = await HtmlTemplate.LoadFromFileAsync(templatePath);
        htmlTemplate.Strict = parsedCli.Arguments.TryGetArgument<bool>("strict", out var strict) && strict;
        var html = htmlTemplate.GenerateHtml(info)!;

        if (parsedCli.Arguments.TryGetArgument<string>("output", out var output))
        {
            using var file = new StreamWriter(output);
            await file.WriteAsync(html);
        }

        Console.WriteLine(html);

        return ICliActionResult.Success;
    }
}
