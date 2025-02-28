
using ProjectLogging.Skills;



namespace ProjectLogging.Records;



public interface IRecord
{
    string? ShortDescription { get; }
    List<string> Points { get; }
    List<Skill> Skills { get; }
    string Location { get; }
    DateOnly StartDate { get; }
    DateOnly? EndDate { get;  }
}
