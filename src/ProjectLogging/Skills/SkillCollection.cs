
using System.Collections;
using System.Text.Json;



namespace ProjectLogging.Skills;



public class SkillCollection(Dictionary<string, List<string>> categorySkills) : IEnumerable, IEnumerable<Category>
{
    public Dictionary<string, List<string>> CategoryNames { get; set; } = categorySkills;



    public SkillCollection() : this([]) { }



    public void AddSkills(List<ISkillData> skillData, bool addNewCategories = false)
    {
        foreach (var data in skillData)
        {
            foreach (var skill in data.Skills)
            {
                if (!CategoryNames.TryGetValue(skill.Category, out var categorySkills))
                {
                    if (!addNewCategories) break;
                    categorySkills = [];
                    CategoryNames.Add(skill.Category, categorySkills);
                }

                // Can't use a set, need to preserve order
                if (!categorySkills.Contains(skill.Name))
                {
                    categorySkills.Add(skill.Name);
                }
            }
        }
    }



    public static async Task<SkillCollection> LoadSkillsAsync(string filePath)
        => await LoadSkillsAsync(File.OpenRead(filePath));

    public static async Task<SkillCollection> LoadSkillsAsync(Stream stream)
    {
        var categorySkills = await JsonSerializer.DeserializeAsync<Dictionary<string, List<string>>>(stream);

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
