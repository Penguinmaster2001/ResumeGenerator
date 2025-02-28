
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
        if (args.Length < 7)
        {
            Console.WriteLine($@"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json>
                <hobbies json> <skills json>");
            return;
        }


        var personalInfo = RecordLoader.LoadPersonalInfoAsync(args[1]);

        var jobs = RecordLoader.LoadRecordsAsync<Job>(args[2]);
        var projects = RecordLoader.LoadRecordsAsync<Project>(args[3]);
        var volunteers = RecordLoader.LoadRecordsAsync<Volunteer>(args[4]);

        var hobbies = SkillCollection.LoadSkillsAsync(args[5]);
        var skills = SkillCollection.LoadSkillsAsync(args[6]);

        await Task.WhenAll(personalInfo, jobs, projects, volunteers, skills, hobbies);

        ResumeGenerator rGen = new();
        rGen.GenerateResume(personalInfo.Result, jobs.Result, projects.Result, volunteers.Result, skills.Result, hobbies.Result)
            .GeneratePdf("test.pdf");
    }
}
