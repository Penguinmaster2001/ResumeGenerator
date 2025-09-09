
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;



namespace ProjectLogging.ResumeGeneration;



public class ResumeModelCreator
{
    public PersonalInfo PersonalInfo;

    public List<(string Name, IEnumerable<IModel> Categories)> Segments;



    public ResumeModelCreator(PersonalInfo personalInfo,
        IEnumerable<(string Name, IEnumerable<IModel> Models)> segments)
    {
        PersonalInfo = personalInfo;
        Segments = [.. segments];
    }



    public ResumeModel CreateModel()
    {
        ResumeHeaderModel resumeHeader = new(PersonalInfo);

        var resumeSegments = Segments.Select(
                                        s => new ResumeSegmentModel(s.Name, s.Categories.Select(c => ResumeEntryFactory.CreateEntry(c))))
                                     .ToList();

        ResumeBodyModel resumeBody = new(resumeSegments);

        return new(resumeHeader, resumeBody);
    }
}
