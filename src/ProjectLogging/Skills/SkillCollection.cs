
using System.Collections;
using System.Text.Json;



namespace ProjectLogging.Skills;



public class SkillCollection(Dictionary<string, HashSet<string>> categorySkills) : IEnumerable, IEnumerable<Category>
{
    public Dictionary<string, HashSet<string>> CategoryNames { get; set; } = categorySkills;



    public SkillCollection() : this([]) { }



    public void AddSkills(List<ISkillData> skillData, bool addNewCategories = false)
    {
        foreach (var data in skillData)
        {
            foreach (var skill in data.Skills)
            {
                if (!CategoryNames.TryGetValue(skill.Category, out var value))
                {
                    if (!addNewCategories) break;
                    value = [];
                    CategoryNames.Add(skill.Category, value);
                }

                value.Add(skill.Name);
            }
        }
    }



    public static async Task<SkillCollection> LoadSkillsAsync(string filePath)
        => await LoadSkillsAsync(File.OpenRead(filePath));

    public static async Task<SkillCollection> LoadSkillsAsync(Stream stream)
    {
        var categorySkills = await JsonSerializer.DeserializeAsync<Dictionary<string, HashSet<string>>>(stream);

        return new(categorySkills ?? []);
    }



    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Category> GetEnumerator()
    {
        foreach (string category in CategoryNames.Keys)
        {
            yield return new Category(category, CategoryNames[category]);
        }
    }
}
