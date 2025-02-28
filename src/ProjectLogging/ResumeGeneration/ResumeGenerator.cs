
using QuestPDF.Infrastructure;

using ProjectLogging.Records;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public class ResumeGenerator
{
    static ResumeGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts/");
    }



    public ResumeDocument GenerateResume(PersonalInfo personalInfo,
                                         List<Job> jobs, List<Project> projects, List<Volunteer> volunteers,
                                         SkillCollection hobbies)
    {
        SkillCollection skillCollection = new();
        skillCollection.AddSkills(jobs);
        skillCollection.AddSkills(projects);
        skillCollection.AddSkills(volunteers);

        ResumeModel model = new ResumeModelCreator(personalInfo, skillCollection.CategorySkills,
            hobbies.CategorySkills, jobs, projects, volunteers).CreateModel();

        return new(model);
    }
}
