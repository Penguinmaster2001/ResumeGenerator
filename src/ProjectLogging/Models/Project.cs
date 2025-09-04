
using System.Text.Json.Serialization;

using ProjectLogging.Skills;



namespace ProjectLogging.Models;



[JsonSerializable(typeof(Project))]
public record Project(string Title, string ShortDescription, List<string> Points, List<Skill> Skills,
    string Location, DateOnly StartDate, DateOnly? EndDate) : BaseModel(ShortDescription,
                        Points,
                        Skills,
                        Location,
                        StartDate,
                        EndDate);
