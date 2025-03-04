
using System.Text.Json.Serialization;

using ProjectLogging.Skills;



namespace ProjectLogging.Records;



[JsonSerializable(typeof(Volunteer))]
public record Volunteer(string Organization, string Position, string ShortDescription, List<string> Points,
    List<Skill> Skills, string Location, DateOnly StartDate, DateOnly? EndDate) : IRecord;
