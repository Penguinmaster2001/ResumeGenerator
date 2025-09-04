
using ProjectLogging.Models;
using ProjectLogging.Skills;
using QuestPDF.Infrastructure;



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
        (string Name, SkillCollection Skills) skillsSegment,
        params IEnumerable<(string Name, IEnumerable<IModel> Model)> segments)
    {
        foreach (IEnumerable<BaseModel> info in segments.Select(s => s.Model).OfType<IEnumerable<BaseModel>>())
        {
            skillsSegment.Skills.AddSkills(info.ToList());
        }

        ResumeModel model = new ResumeModelCreator(personalInfo, segments.Prepend(skillsSegment)).CreateModel();

        return new(model);
    }
}
