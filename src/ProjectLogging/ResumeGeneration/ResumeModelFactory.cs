
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeModelFactory
{
    public static ResumeModel GenerateResume(IDataCollection data)
    {
        var skills = data.GetData<SkillCollection>(data.DataConfig.Skills.Title);

        // foreach (var skillData in data.GetDataOfType<IEnumerable<ISkillData>>().Select(s => s.data))
        // {
        //     skills.Aggregate(skillData, false);
        // }

        ResumeHeaderModel resumeHeader = new(data.GetData<PersonalInfo>(data.DataConfig.PersonalInfo.Title));

        var skillSegment = new ResumeSegmentModel(data.DataConfig.Skills.Title);
        foreach (var category in skills.Categories)
        {
            skillSegment.Entries.Add(ResumeEntryFactory.CreateEntry(category));
        }

        // var hobbySegment = new ResumeSegmentModel("hobbies");
        // foreach (var category in data.GetData<SkillCollection>("hobbies").Categories)
        // {
        //     hobbySegment.Entries.Add(ResumeEntryFactory.CreateEntry(category));
        // }

        var educationSegment = CreateModel<List<Education>>(data.DataConfig.Education.Title, data);
        foreach (var category in data.GetData<SkillCollection>(data.DataConfig.Courses.Title).Categories)
        {
            educationSegment.Entries.Add(ResumeEntryFactory.CreateEntry(category));
        }

        var volunteerSegment = CreateModel<List<Volunteer>>(data.DataConfig.Volunteering.Title, data);
        var careerSegment = CreateModel<List<Job>>(data.DataConfig.Jobs.Title, data);
        var projectSegment = CreateModel<List<Project>>(data.DataConfig.Projects.Title, data);

        var resumeSegments = new List<ResumeSegmentModel>
            {
                skillSegment,
                educationSegment,
                careerSegment,
                projectSegment,
                volunteerSegment,
                // hobbySegment,
            };

        ResumeBodyModel resumeBody = new(resumeSegments);

        return new(resumeHeader, resumeBody);
    }



    private static ResumeSegmentModel CreateModel<T>(string label, IDataCollection data) where T : IEnumerable<object>
    {
        return new ResumeSegmentModel(label, data.GetData<T>(label).Select(ResumeEntryFactory.CreateEntry));
    }
}
