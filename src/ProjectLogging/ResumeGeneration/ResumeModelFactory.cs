
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeModelFactory
{
    public static ResumeModel GenerateResume(PersonalInfo personalInfo,
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
        
        return new(resumeHeader, resumeBody);
    }
}
