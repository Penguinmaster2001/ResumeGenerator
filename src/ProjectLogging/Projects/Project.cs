
using System.Text.Json.Serialization;



namespace ProjectLogging.Projects;



[JsonSerializable(typeof(Project))]
public record Project(string Title, string ShortDescription, List<string> Points, List<Skill> Skills,
    string Location, DateOnly StartDate, DateOnly? EndDate);
