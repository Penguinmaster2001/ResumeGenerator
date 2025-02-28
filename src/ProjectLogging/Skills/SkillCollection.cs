
using System.Text.Json;

using ProjectLogging.Records;



namespace ProjectLogging.Skills;



public class SkillCollection(Dictionary<string, HashSet<string>> categorySkills)
{
    public Dictionary<string, HashSet<string>> CategorySkills = categorySkills;



    public SkillCollection() : this(new()) { }



    public void AddSkills<T>(List<T> records, bool addNewCategories = false) where T : IRecord
    {
        foreach (IRecord record in records)
        {
            foreach (Skill skill in record.Skills)
            {
                if (!CategorySkills.ContainsKey(skill.Category))
                {
                    if (!addNewCategories) break;

                    CategorySkills.Add(skill.Category, new());
                }

                CategorySkills[skill.Category].Add(skill.Name);
            }
        }
    }



    public static async Task<SkillCollection> LoadSkillsAsync(string filePath)
        => await LoadSkillsAsync(File.OpenRead(filePath));

    public static async Task<SkillCollection> LoadSkillsAsync(Stream stream)
    {
        var categorySkills = await JsonSerializer.DeserializeAsync<Dictionary<string, HashSet<string>>>(stream);

        return new(categorySkills ?? new());
    }
}
