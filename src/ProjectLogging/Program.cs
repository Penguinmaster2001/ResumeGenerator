
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

        var filteredModel = FilterResume(resumeModel);

        var resumePath = Path.ChangeExtension(args[9], null);

        GeneratePdf(resumeModel, resumePath);
        GeneratePdf(filteredModel, resumePath + "Filtered");
        // GenerateWebsite(resumeModel, args[10]);
    }



    private static ResumeModel FilterResume(ResumeModel model)
    {
        var config = new AiFilterConfig("../testing/AiModels/all-MiniLM-L6-v2/model.onnx",
            "../testing/AiModels/all-MiniLM-L6-v2/vocab.txt",
            "../testing/AiModels/jobDescription.txt",
            -1,
            new()
            {
                {"work experience", 2},
                {"projects", 2},
                {"volunteer / extracurricular", 1},
            },
            -1,
            new()
            {
                {"tech skills", 5},
                {"education", 3},
                {"hobbies", 3},
            });

        using var jobDescriptionFile = File.OpenText(config.jobDescriptionPath);
        var jobDescription = jobDescriptionFile.ReadToEnd();

        var filter = new EmbeddingFilter(config);

        Console.WriteLine("Filtering");
        var filtered = filter.FilterData(model.ResumeBody.ResumeSegments, jobDescription);
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
