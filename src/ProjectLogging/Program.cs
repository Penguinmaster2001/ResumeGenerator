
using ProjectLogging.Projects;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.Skills;
using ProjectLogging.WebsiteGeneration;
using QuestPDF.Fluent;
using ProjectLogging.Data;



namespace ProjectLogging;



public static class Program
{
    public static async Task Main()
    {
        await GenerateResume();
        // await GenerateWebsite();
    }



    private static async Task GenerateResume()
    {
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 10)
        {
            Console.WriteLine($"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json> "
                + "<education json> <courses json> <hobbies json> <skills json> <resume output>");
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

        ResumeGenerator.GenerateResume(personalInfo.Result,
                            ("tech skills", skills.Result),
                            ("volunteer / extracurricular", volunteers.Result),
                            ("hobbies", hobbies.Result),
                            ("education", education.Result.Select(e => e as object).Concat(courses.Result)),
                            ("work experience", jobs.Result),
                            ("projects", projects.Result))
            .GeneratePdf(args[9]);
    }



    public static async Task GenerateWebsite()
    {
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 2)
        {
            Console.WriteLine($"Usage: {args[0]} <website directory>");
            return;
        }

        await Task.Run(() => WebsiteGenerator.GenerateWebsite().CreateFiles(args[1]));
    }
}
