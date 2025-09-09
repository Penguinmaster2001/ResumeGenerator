
using System.Text.Json.Serialization;
using ProjectLogging.Skills;



namespace ProjectLogging.Data;



[JsonSerializable(typeof(Education))]
public record Education(string School,
                        string Degree,
                        string? ShortDescription,
                        List<string> Points,
                        List<string> RelevantCourses,
                        List<Skill> Skills,
                        string Location,
                        DateOnly StartDate,
                        DateOnly? EndDate)
                : BaseData(ShortDescription,
                    Points,
                    Skills,
                    Location,
                    StartDate,
                    EndDate);
