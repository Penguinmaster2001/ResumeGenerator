
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
                    // DESIGN ISSUE: The break statement here exits the inner loop (Skills loop), not the
                    // outer loop (skillData loop). This means if a skill's category doesn't exist and
                    // addNewCategories is false, we skip ALL remaining skills for this data item, even
                    // if subsequent skills belong to existing categories. This is likely a logic bug.
                    // Consider using 'continue' instead of 'break', or restructure to check categories
                    // properly. This could cause skills to be silently dropped.
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
