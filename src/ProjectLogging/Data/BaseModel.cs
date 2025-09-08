
using ProjectLogging.Skills;



namespace ProjectLogging.Data;



public record BaseModel(string? ShortDescription,
                        List<string> Points,
                        List<Skill> Skills,
                        string Location,
                        DateOnly StartDate,
                        DateOnly? EndDate)
                    : IModel;
