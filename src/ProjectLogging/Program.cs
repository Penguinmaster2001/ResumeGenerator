
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
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 11)
        {
            Console.WriteLine($"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json> "
                + "<education json> <courses json> <hobbies json> <skills json> <resume output> <website output>");
            return;
        }

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

        // var filteredModel = FilterResume(resumeModel, args[11]);
        // GeneratePdf(filteredModel, resumePath + "Filtered");

        // GenerateWebsite(resumeModel, args[10]);
    }



    private static ResumeModel FilterResume(ResumeModel model, string configPath)
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
        // var styleManager = new PdfStyleManager();
        
        var styleManager = new PdfStyleManager()
        {
            PageColor = Color.FromHex("#F7F5F0"),
            SegmentHeaderColor = Color.FromHex("#0B3D91"),
            ResumeHeaderTextColor = Color.FromHex("#0B3D91"),
            NameTextColor = Color.FromHex("#0B3D91"),
            AccentColor = Color.FromHex("#6B7280"),
            TextColor = Color.FromHex("#000000"),
            BulletPointColors = [
                    Color.FromHex("#323846"),
                    Color.FromHex("#001315"),
                ],
            FontFamily = "Ubuntu Condensed",
        };

        // var styleManager = new PdfStyleManager()
        // {
        //     PageColor = Colors.White,
        //     SegmentHeaderColor = Colors.Green.Darken3,
        //     ResumeHeaderTextColor = Colors.Blue.Darken4,
        //     NameTextColor = Colors.Blue.Accent2,
        //     AccentColor = Colors.Black,
        //     TextColor = Colors.Black,
        //     BulletPointColors = [
        //         Color.FromHex("#103610"),
        //         Color.FromHex("#101840"),
        //         ],
        //     FontFamily = "Ubuntu Condensed",
        // };

        // var styleManager = new PdfStyleManager()
        // {
        //     PageColor = Color.FromHex("#FAFAFA"),
        //     SegmentHeaderColor = Color.FromHex("#111827"),
        //     SegmentBackgroundColors = [Color.FromHex("#g0g0g0"), Color.FromHex("#00FFFFFF")],
        //     ResumeHeaderTextColor = Color.FromHex("#111827"),
        //     NameTextColor = Color.FromHex("#FF6B6B"),
        //     AccentColor = Color.FromHex("#EAB308"),
        //     TextColor = Color.FromHex("#374151"),
        //     BulletPointColors = [
        //             Color.FromHex("#4b1010"),
        //             Color.FromHex("#4a3902"),
        //         ],
        //     FontFamily = "Ubuntu Condensed",
        // };

        // var styleManager = new PdfStyleManager()
        // {
        //     PageColor = Color.FromHex("#FFFFFF"),
        //     SegmentHeaderColor = Color.FromHex("#312e81"),
        //     ResumeHeaderTextColor = Color.FromHex("#06b6d4"),
        //     NameTextColor = Color.FromHex("#312e81"),
        //     AccentColor = Color.FromHex("#84cc16"),
        //     TextColor = Color.FromHex("#0f172a"),
        //     BulletPointColors = [
        //             Color.FromHex("#06b6d4"),
        //             Color.FromHex("#312e81"),
        //             Color.FromHex("#84cc16"),
        //         ],
        //     FontFamily = "Ubuntu Condensed",
        // };

        // var styleManager = new PdfStyleManager()
        // {
        //     PageColor = Color.FromHex("#F7F5F0"),
        //     SegmentHeaderColor = Color.FromHex("#6e6e73"),
        //     ResumeHeaderTextColor = Color.FromHex("#6e6e73"),
        //     NameTextColor = Color.FromHex("#4a1f23"),
        //     AccentColor = Color.FromHex("#4a1f23"),
        //     TextColor = Color.FromHex("#111111"),
        //     BulletPointColors = [
        //             Color.FromHex("#111111"),
        //         ],

        //     FontFamily = "URW Gothic"
        // };

        var viewFactory = new ViewFactory<Action<IContainer>>();
        viewFactory.AddStrategy<ResumeSegmentViewStrategy>();
        viewFactory.AddStrategy<ResumeHeaderViewStrategy>();
        viewFactory.AddStrategy<ResumeEntryViewStrategy>();
        viewFactory.AddStrategy<ResumeBodyOneColumnViewStrategy>();

        viewFactory.AddHelper<IPdfStyleManager>(styleManager);

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
