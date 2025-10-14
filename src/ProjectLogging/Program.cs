
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration;
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
        dataCollection.AddData("volunteer / extracurricular", volunteers.Result);
        dataCollection.AddData("hobbies", hobbies.Result);
        dataCollection.AddData("education", education.Result);
        dataCollection.AddData("courses", courses.Result);
        dataCollection.AddData("work experience", jobs.Result);
        dataCollection.AddData("projects", projects.Result);

        var resumeModel = ResumeModelFactory.GenerateResume(dataCollection);

        GeneratePdf(resumeModel, args[9]);
        GenerateWebsite(resumeModel, args[10]);
    }



    private static void GeneratePdf(ResumeModel resumeModel, string outDir)
    {
        var viewFactory = new ViewFactory<Action<IContainer>>();
        viewFactory.AddStrategy<ResumeSegmentViewStrategy>();
        viewFactory.AddStrategy<ResumeHeaderViewStrategy>();
        viewFactory.AddStrategy<ResumeEntryViewStrategy>();
        viewFactory.AddStrategy<ResumeBodyViewStrategy>();

        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts/");

        new ResumeDocument(resumeModel, viewFactory).GeneratePdf(outDir);
    }



    public static void GenerateWebsite(ResumeModel resumeModel, string outDir)
    {
        WebsiteGenerator.GenerateWebsite(resumeModel, outDir).CreateFiles();
    }
}
