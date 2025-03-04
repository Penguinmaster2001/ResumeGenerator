
using System.Text.Json;
using System.Collections;

using ProjectLogging.Records;



namespace ProjectLogging.Skills;



public class SkillCollection(Dictionary<string, HashSet<string>> categorySkills) : IEnumerable<Category>
{
    public Dictionary<string, HashSet<string>> CategoryNames { get; set; } = categorySkills;



    public SkillCollection() : this(new()) { }



    public void AddSkills<T>(List<T> records, bool addNewCategories = false) where T : IRecord
    {
        foreach (IRecord record in records)
        {
            foreach (Skill skill in record.Skills)
            {
                if (!CategoryNames.ContainsKey(skill.Category))
                {
                    if (!addNewCategories) break;

                    CategoryNames.Add(skill.Category, new());
                }

                CategoryNames[skill.Category].Add(skill.Name);
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



    public IEnumerator<Category> GetEnumerator()
    {
        foreach (string category in CategoryNames.Keys)
        {
            yield return new Category(category, CategoryNames[category]);
        }
    }



    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
