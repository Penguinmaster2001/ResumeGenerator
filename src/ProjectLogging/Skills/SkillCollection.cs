
using System.Text.Json;
using System.Collections;
using ProjectLogging.Data;



namespace ProjectLogging.Skills;



public class SkillCollection(Dictionary<string, HashSet<string>> categorySkills) : IEnumerable, IEnumerable<Category>, IModel
{
    public Dictionary<string, HashSet<string>> CategoryNames { get; set; } = categorySkills;



    public SkillCollection() : this([]) { }



    public void AddSkills(List<ISkillData> records, bool addNewCategories = false)
    {
        foreach (var record in records)
        {
            foreach (Skill skill in record.Skills)
            {
                if (!CategoryNames.TryGetValue(skill.Category, out HashSet<string>? value))
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
