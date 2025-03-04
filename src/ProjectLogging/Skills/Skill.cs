
using System.Text.Json.Serialization;



namespace ProjectLogging.Skills;



[JsonSerializable(typeof(Skill))]
public record Skill(string Category, string Name);
