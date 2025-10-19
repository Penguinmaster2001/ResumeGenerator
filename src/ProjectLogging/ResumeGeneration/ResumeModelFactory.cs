
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeModelFactory
{
    public static ResumeModel GenerateResume(IDataCollection data)
    {
        var skills = data.GetData<SkillCollection>("tech skills");

        foreach (var skillData in data.GetDataOfType<IEnumerable<ISkillData>>().Select(s => s.data))
        {
            skills.AddSkills([.. skillData]);
        }

        ResumeHeaderModel resumeHeader = new(data.GetData<PersonalInfo>("personal info"));

        var skillSegment = new ResumeSegmentModel("tech skills");
        foreach (var category in skills.CategoryNames)
        {
            skillSegment.Entries.Add(ResumeEntryFactory.CreateEntry(new Category(category.Key, category.Value)));
        }

        var hobbySegment = new ResumeSegmentModel("hobbies");
        foreach (var category in data.GetData<SkillCollection>("hobbies").CategoryNames)
        {
            hobbySegment.Entries.Add(ResumeEntryFactory.CreateEntry(new Category(category.Key, category.Value)));
        }

        var educationSegment = CreateModel<List<Education>>("education", data);
        foreach (var category in data.GetData<SkillCollection>("courses").CategoryNames)
        {
            educationSegment.Entries.Add(ResumeEntryFactory.CreateEntry(new Category(category.Key, category.Value)));
        }

        var volunteerSegment = CreateModel<List<Volunteer>>("volunteer / extracurricular", data);
        var careerSegment = CreateModel<List<Job>>("work experience", data);
        var projectSegment = CreateModel<List<Project>>("projects", data);

        var resumeSegments = new List<ResumeSegmentModel>
            {
                skillSegment,
                educationSegment,
                careerSegment,
                projectSegment,
                volunteerSegment,
                hobbySegment,
            };

        ResumeBodyModel resumeBody = new(resumeSegments);

        return new(resumeHeader, resumeBody);
    }



    private static ResumeSegmentModel CreateModel<T>(string label, IDataCollection data) where T : IEnumerable<object>
    {
        return new ResumeSegmentModel(label, data.GetData<T>(label).Select(ResumeEntryFactory.CreateEntry));
    }
}
