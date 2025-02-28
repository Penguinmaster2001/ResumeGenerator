
using QuestPDF.Fluent;

using ProjectLogging.Records;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.Projects;
using ProjectLogging.Skills;



namespace ProjectLogging;



public static class Program
{
    public static async Task Main()
    {
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 9)
        {
            Console.WriteLine($"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json> "
                + "<education json> <courses json> <hobbies json> <skills json>");
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

        ResumeGenerator rGen = new();
        rGen.GenerateResume(personalInfo.Result, jobs.Result, projects.Result, volunteers.Result,
            education.Result, courses.Result, skills.Result, hobbies.Result)
            .GeneratePdf("test.pdf");
    }
}
