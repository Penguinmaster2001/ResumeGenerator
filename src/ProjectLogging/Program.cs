
using System;

using QuestPDF.Fluent;

using ProjectLogging.Records;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.Projects;



namespace ProjectLogging;



public static class Program
{
    public static async Task Main()
    {
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length < 5)
        {
            Console.WriteLine($"Usage: {args[0]} <personal info json> <job json> <project json> <volunteer json>");
            return;
        }


        var personalInfo = RecordLoader.LoadPersonalInfoAsync(File.OpenRead(args[1]));

        var jobs = RecordLoader.LoadRecordsAsync<Job>(File.OpenRead(args[2]));
        var projects = RecordLoader.LoadRecordsAsync<Project>(File.OpenRead(args[3]));
        var volunteers = RecordLoader.LoadRecordsAsync<Volunteer>(File.OpenRead(args[4]));

        await Task.WhenAll(personalInfo, jobs, projects, volunteers);

        ResumeGenerator rGen = new();
        rGen.GenerateResume(personalInfo.Result, jobs.Result, projects.Result, volunteers.Result)
            .GeneratePdf("test.pdf");
    }
}
