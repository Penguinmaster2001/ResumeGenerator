
using System.Text.Json;

namespace ProjectLogging.Skills;



public class SkillCollection
{
    private Dictionary<int, Skill> _skills;

    private Dictionary<string, List<string>> _categorySkills;



    public SkillCollection()
    {
        _skills = new();

        _categorySkills = new();
    }



    public void LoadSkills(string filePath)
    {
        string jsonText = File.ReadAllText(filePath);

        _categorySkills = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonText) ?? new();

        foreach (string category in _categorySkills.Keys)
        {
            foreach (string name in _categorySkills[category])
            {
                Skill skill = new(category, name);

                _skills.Add(skill.ID, skill);
            }
        }
    }
}
