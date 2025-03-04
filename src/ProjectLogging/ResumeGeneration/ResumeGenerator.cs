
using QuestPDF.Infrastructure;

using ProjectLogging.Skills;
using ProjectLogging.Records;



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
        (string SkillsSegmentName, SkillCollection Skills) skillsSegment,
        params IEnumerable<(string Name, IEnumerable<IResumeEntryable> info)> segments)
    {
        foreach (IEnumerable<IRecord> info in segments.Select(s => s.info).OfType<IEnumerable<IRecord>>())
        {
            skillsSegment.Skills.AddSkills(info.ToList());
        }

        ResumeModel model = new ResumeModelCreator(personalInfo, segments.Prepend(skillsSegment)).CreateModel();

        return new(model);
    }
}
