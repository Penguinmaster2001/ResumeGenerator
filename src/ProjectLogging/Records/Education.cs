
using System.Text.Json.Serialization;

using ProjectLogging.Skills;



namespace ProjectLogging.Records;



[JsonSerializable(typeof(Education))]
public record Education(string School, string Degree, string? ShortDescription, List<string> Points,
    List<string> ReleventCourses, List<Skill> Skills,
    string Location, DateOnly StartDate, DateOnly? EndDate) : IRecord;
