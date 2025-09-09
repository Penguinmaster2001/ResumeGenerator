
using ProjectLogging.Skills;



namespace ProjectLogging.Data;



public record BaseData(string? ShortDescription,
                        List<string> Points,
                        List<Skill> Skills,
                        string Location,
                        DateOnly StartDate,
                        DateOnly? EndDate)
                    : ISkillData
{
    IEnumerable<Skill> ISkillData.Skills { get => Skills; }
}

