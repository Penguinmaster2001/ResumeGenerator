
using System.Text.Json.Serialization;
using ProjectLogging.Skills;



namespace ProjectLogging.Data;



[JsonSerializable(typeof(Job))]
public record Job(string Company,
                  string Position,
                  string ShortDescription,
                  List<string> Points,
                  SkillCollection Skills,
                  string Location,
                  DateOnly StartDate,
                  DateOnly? EndDate)
            : BaseData(ShortDescription,
                        Points,
                        Skills,
                        Location,
                        StartDate,
                        EndDate);
