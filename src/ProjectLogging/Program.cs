
using System.Text.Json;
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.ResumeGeneration.Filtering;
using ProjectLogging.Skills;
using ProjectLogging.Views.Pdf;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging;



public static class Program
{
    public static async Task Main()
    {
        // DESIGN ISSUE: Hard-coded argument count and indices make this brittle and error-prone.
        // Consider using a command-line parsing library (e.g., System.CommandLine) or a configuration
        // object to make argument handling more maintainable and self-documenting.
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 11)
        {
            Console.WriteLine($"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json> "
                + "<education json> <courses json> <hobbies json> <skills json> <resume output> <website output>");
            return;
        }

        // DESIGN ISSUE: Magic numbers for array indices make the code difficult to understand and maintain.
        // If argument order changes, multiple locations must be updated. Consider using named constants
        // or an enum to make the intent clear (e.g., const int PERSONAL_INFO_INDEX = 1).
        var personalInfo = RecordLoader.LoadPersonalInfoAsync(args[1]);

        var jobs = RecordLoader.LoadRecordsAsync<Job>(args[2]);
        var projects = RecordLoader.LoadRecordsAsync<Project>(args[3]);
        var volunteers = RecordLoader.LoadRecordsAsync<Volunteer>(args[4]);

        var education = RecordLoader.LoadRecordsAsync<Education>(args[5]);
        var courses = SkillCollection.LoadSkillsAsync(args[6]);

        var hobbies = SkillCollection.LoadSkillsAsync(args[7]);
        var skills = SkillCollection.LoadSkillsAsync(args[8]);

        await Task.WhenAll(personalInfo, jobs, projects, volunteers, education, courses, skills, hobbies);

        var dataCollection = new DataCollection();

        dataCollection.AddData("personal info", personalInfo.Result);
        dataCollection.AddData("tech skills", skills.Result);
        dataCollection.AddData("education", education.Result);
        dataCollection.AddData("courses", courses.Result);
        dataCollection.AddData("work experience", jobs.Result);
        dataCollection.AddData("projects", projects.Result);
        dataCollection.AddData("volunteer / extracurricular", volunteers.Result);
        dataCollection.AddData("hobbies", hobbies.Result);

        var resumeModel = ResumeModelFactory.GenerateResume(dataCollection);

        var resumePath = Path.ChangeExtension(args[9], null);
        GeneratePdf(resumeModel, resumePath);

        // DESIGN ISSUE: args[11] is accessed but the length check only verifies 11 arguments (indices 0-10).
        // This will throw an IndexOutOfRangeException if exactly 11 arguments are provided.
        // The check should be "args.Length < 12" to access args[11] safely.
        var filteredModel = FilterResume(resumeModel, args[11]);

        GeneratePdf(filteredModel, resumePath + "Filtered");
        // GenerateWebsite(resumeModel, args[10]);
    }



    private static ResumeModel FilterResume(ResumeModel model, string configPath)
    {
        // DESIGN ISSUE: Catching generic Exception hides specific errors and makes debugging difficult.
        // Consider catching specific exceptions (JsonException, FileNotFoundException, IOException)
        // to handle different error scenarios appropriately. Also, silently returning the original
        // model on error could mask configuration problems that should be reported to users.
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

        var diversityRanker = new DiversityRanker(config);

        Console.WriteLine("Filtering");
        var filtered = diversityRanker.FilterResume(model.ResumeBody.ResumeSegments);
        Console.WriteLine("Filtering done");

        var filteredBody = new ResumeBodyModel(filtered);
        var filteredModel = new ResumeModel(model.ResumeHeader, filteredBody);

        return filteredModel;
    }



    private static void GeneratePdf(ResumeModel resumeModel, string outDir)
    {
        var viewFactory = new ViewFactory<Action<IContainer>>();
        viewFactory.AddStrategy<ResumeSegmentViewStrategy>();
        viewFactory.AddStrategy<ResumeHeaderViewStrategy>();
        viewFactory.AddStrategy<ResumeEntryViewStrategy>();
        viewFactory.AddStrategy<ResumeBodyOneColumnViewStrategy>();

        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts/");

        var path = Path.ChangeExtension(outDir, "pdf");

        new ResumeDocument(resumeModel, viewFactory).GeneratePdf(path);
    }



    public static void GenerateWebsite(ResumeModel resumeModel, string outDir)
    {
        WebsiteGenerator.GenerateWebsite(resumeModel, outDir).CreateFiles();
    }
}
