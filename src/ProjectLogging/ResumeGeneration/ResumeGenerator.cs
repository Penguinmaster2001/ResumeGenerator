
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
        (string Name, SkillCollection Skills) skillInfo,
        params IEnumerable<(string Name, IEnumerable<object> Data)> segmentInfo)
    {
        foreach (var skillData in segmentInfo.Select(s => s.Data).OfType<IEnumerable<ISkillData>>())
        {
            skillInfo.Skills.AddSkills([.. skillData]);
        }

        ResumeHeaderModel resumeHeader = new(personalInfo);

        var resumeSegments = new List<ResumeSegmentModel>();

        var skillSegment = new ResumeSegmentModel(skillInfo.Name);
        foreach (var category in skillInfo.Skills.CategoryNames)
        {
            skillSegment.Entries.Add(ResumeEntryFactory.CreateEntry(new Category(category.Key, [.. category.Value])));
        }

        resumeSegments.Add(skillSegment);

        foreach (var segment in segmentInfo)
        {
            resumeSegments.Add(new ResumeSegmentModel(segment.Name, segment.Data.Select(ResumeEntryFactory.CreateEntry)));
        }
    
        ResumeBodyModel resumeBody = new(resumeSegments);
        ResumeModel model = new(resumeHeader, resumeBody);

        var viewFactory = new PdfViewFactory();
        viewFactory.AddStrategy(new ResumeHeaderViewStrategy());
        viewFactory.AddStrategy(new ResumeBodyViewStrategy());
        viewFactory.AddStrategy(new ResumeSegmentViewStrategy());
        viewFactory.AddStrategy(new ResumeEntryViewStrategy());

        return new(model, viewFactory);
    }
}
