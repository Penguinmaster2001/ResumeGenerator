
namespace ProjectLogging.Skills;



public record Skill(string Category, string Name)
{
    public static int GetSkillID(string category, string name) => HashCode.Combine(category, name);



    public int ID { get; init; } = GetSkillID(Category, Name);
}
