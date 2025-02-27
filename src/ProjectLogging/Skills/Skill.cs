
using System.Text.Json.Serialization;



namespace ProjectLogging.Skills;



[JsonSerializable(typeof(Skill))]
public record Skill(string Category, string Name)
{
    public static int GetSkillID(string category, string name) => HashCode.Combine(category, name);



    [JsonIgnore]
    public int ID { get; init; } = GetSkillID(Category, Name);
}
