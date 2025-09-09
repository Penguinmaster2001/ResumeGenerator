
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Skills;
using ProjectLogging.Views.Pdf;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeGenerator
{
    static ResumeGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts/");
    }



    public static ResumeDocument GenerateResume(PersonalInfo personalInfo,
        (string Name, SkillCollection Skills) skillsSegment,
        params IEnumerable<(string Name, IEnumerable<IModel> Model)> segments)
    {
        foreach (var skillData in segments.Select(s => s.Model).OfType<IEnumerable<ISkillData>>())
        {
            skillsSegment.Skills.AddSkills([.. skillData]);
        }

        ResumeModel model = new ResumeModelCreator(personalInfo, segments.Prepend(skillsSegment)).CreateModel();

        var viewFactory = new PdfViewFactory();
        viewFactory.AddStrategy(new ResumeHeaderViewStrategy());
        viewFactory.AddStrategy(new ResumeBodyViewStrategy());
        viewFactory.AddStrategy(new ResumeSegmentViewStrategy());

        return new(model, viewFactory);
    }
}
