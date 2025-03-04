
using ProjectLogging.Skills;
using ProjectLogging.ResumeGeneration;



namespace ProjectLogging.Records;



public interface IRecord : IResumeEntryable
{
    string? ShortDescription { get; }
    List<string> Points { get; }
    List<Skill> Skills { get; }
    string Location { get; }
    DateOnly StartDate { get; }
    DateOnly? EndDate { get; }


    ResumeEntry IResumeEntryable.ToResumeEntry() => ResumeEntryFactory.CreateEntry(this);
}
