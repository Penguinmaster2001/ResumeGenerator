
using ProjectLogging.Models;
using ProjectLogging.Views.Resume;



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
        ResumeHeaderComponent resumeHeader = new(PersonalInfo);

        var resumeSegments = Segments.Select(
                                        s => new ResumeSegmentComponent(s.Name, s.Categories.Select(c => ResumeEntryFactory.CreateEntry(c))))
                                     .ToList();

        ResumeBodyComponent resumeBody = new(resumeSegments);

        return new(resumeHeader, resumeBody);
    }
}
