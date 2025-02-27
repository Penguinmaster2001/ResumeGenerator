
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeGenerator
{
    static ResumeGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }



    public ResumeDocument GenerateResume(PersonalInfo personalInfo,
        List<Job> jobs, List<Project> projects, List<Volunteer> volunteers)
    {
        ResumeModel model = new ResumeModelCreator(personalInfo, jobs, projects, volunteers).CreateModel();

        return new(model);
    }
}
