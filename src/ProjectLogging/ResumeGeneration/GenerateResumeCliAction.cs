
using System.Text.Json;
using ProjectLogging.Cli;
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration.Filtering;
using ProjectLogging.ResumeGeneration.Styling;
using ProjectLogging.Skills;
using ProjectLogging.Views.Pdf;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public static class GenerateResumeCliAction
{
    public static CliAction CliAction => new("generate", "resume", "Generate a resume", GenerateResumeAsync, new([
        CliArgument.Create("settings", "s", true, "Path to settings file.", (s) => s, null)
    ]));




    public static async Task<ICliActionResult> GenerateResumeAsync(ParsedCliArguments arguments)
    {
        var settings = JsonSerializer.Deserialize<GenerationSettings>(File.OpenRead(arguments.GetArgument<string>("settings")));

        if (settings is null)
        {
            return new CliActionFailureResult("Unable to read settings.");
        }

        var dataConfig = JsonSerializer.Deserialize<DataConfig>(File.OpenRead(settings.DataConfigPath));

        if (dataConfig is null)
        {
            return new CliActionFailureResult("Unable to read data config.");
        }

        var personalInfo = RecordLoader.LoadPersonalInfoAsync(settings.GetFullPath(dataConfig.PersonalInfo.Path));

        var jobs = RecordLoader.LoadRecordsAsync<Job>(settings.GetFullPath(dataConfig.Jobs.Path));
        var projects = RecordLoader.LoadRecordsAsync<Project>(settings.GetFullPath(dataConfig.Projects.Path));
        var volunteers = RecordLoader.LoadRecordsAsync<Volunteer>(settings.GetFullPath(dataConfig.Volunteering.Path));

        var education = RecordLoader.LoadRecordsAsync<Education>(settings.GetFullPath(dataConfig.Education.Path));
        var courses = SkillCollection.LoadSkillsAsync(settings.GetFullPath(dataConfig.Courses.Path));

        var hobbies = SkillCollection.LoadSkillsAsync(settings.GetFullPath(dataConfig.Hobbies.Path));
        var skills = SkillCollection.LoadSkillsAsync(settings.GetFullPath(dataConfig.Skills.Path));

        await Task.WhenAll(personalInfo, jobs, projects, volunteers, education, courses, skills, hobbies);

        var dataCollection = new DataCollection(dataConfig);

        dataCollection.AddData(dataConfig.PersonalInfo.Title, personalInfo.Result);
        dataCollection.AddData(dataConfig.Skills.Title, skills.Result);
        dataCollection.AddData(dataConfig.Education.Title, education.Result);
        dataCollection.AddData(dataConfig.Courses.Title, courses.Result);
        dataCollection.AddData(dataConfig.Jobs.Title, jobs.Result);
        dataCollection.AddData(dataConfig.Projects.Title, projects.Result);
        dataCollection.AddData(dataConfig.Volunteering.Title, volunteers.Result);
        dataCollection.AddData(dataConfig.Hobbies.Title, hobbies.Result);

        var resumeModel = ResumeModelFactory.GenerateResume(dataCollection);
        GeneratePdf(resumeModel, settings, "resume");

        var filteredModel = FilterResume(resumeModel, settings, dataConfig, settings.AiConfigPath);
        GeneratePdf(filteredModel, settings, "AnthonyCieriResume");

        return new GenerateResumeResult();
    }



    private static ResumeModel FilterResume(ResumeModel model, GenerationSettings settings, DataConfig dataConfig,string configPath)
    {
        AiFilterConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<AiFilterConfig>(File.OpenRead(configPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing config: {ex}");
            config = null;
        }

        if (config is null)
        {
            Console.WriteLine("Unable to load ai config, no filtering done");
            return model;
        }

        if (!config.DoAiFiltering)
        {
            Console.WriteLine("DoAiFiltering false");
            return model;
        }

        var diversityRanker = new DiversityRanker(settings, config);

        Console.WriteLine("Filtering");
        var filtered = diversityRanker.FilterResume(model.ResumeBody.ResumeSegments, dataConfig);
        Console.WriteLine("Filtering done");

        var filteredBody = new ResumeBodyModel(filtered);
        var filteredModel = new ResumeModel(model.ResumeHeader, filteredBody);

        return filteredModel;
    }



    private static void GeneratePdf(ResumeModel resumeModel, GenerationSettings settings, string fileName)
    {
        var styles = JsonSerializer.Deserialize<Dictionary<string, PdfStyleConfig>>(File.OpenRead(settings.PdfStylesPath)) ?? [];

        var styleManager = styles.TryGetValue(settings.PdfStyle, out var styleConfig) ? PdfStyleManager.CreateFromConfig(styleConfig) : new();

        var viewFactory = new ViewFactory<Action<IContainer>>();
        viewFactory.AddStrategy<ResumeSegmentViewStrategy>();
        viewFactory.AddStrategy<ResumeHeaderViewStrategy>();
        viewFactory.AddStrategy<ResumeEntryViewStrategy>();
        viewFactory.AddStrategy<ResumeBodyOneColumnViewStrategy>();

        viewFactory.AddHelper<IPdfStyleManager>(styleManager);

        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts/");

        var path = Path.ChangeExtension(Path.Combine(settings.ResumeOutputPath, fileName), Constants.Resources.Pdf);

        new ResumeDocument(resumeModel, viewFactory).GeneratePdf(path);
    }
}



public class GenerateResumeResult : ICliActionResult
{

}
