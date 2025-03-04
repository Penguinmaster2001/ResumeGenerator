
using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeModelCreator
{
    public PersonalInfo PersonalInfo;

    public List<(string Name, IEnumerable<IResumeEntryable> Info)> Segments;



    public ResumeModelCreator(PersonalInfo personalInfo,
        IEnumerable<(string Name, IEnumerable<IResumeEntryable> info)> segments)
    {
        PersonalInfo = personalInfo;
        Segments = segments.ToList();
    }



    public ResumeModel CreateModel()
    {
        ResumeHeaderComponent resumeHeader = new(PersonalInfo);

        var resumeSegments = Segments.Select(
                                        s => new ResumeSegmentComponent(s.Name, s.Info.Select(i => i.ToResumeEntry())))
                                     .ToList();

        ResumeBodyComponent resumeBody = new(resumeSegments);

        return new(resumeHeader, resumeBody);
    }
}
