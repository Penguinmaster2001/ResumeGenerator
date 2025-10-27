
using System.Text.Json;



namespace ProjectLogging.Skills;



public class SkillCollection
{
    public List<Category> Categories { get; set; } = [];



    public SkillCollection(List<Category> categories)
    {
        Categories = categories;
    }



    public SkillCollection() { }



    public void Aggregate(IEnumerable<ISkillData> skillDataCollection, bool addNewCategories = true)
    {
        foreach (var data in skillDataCollection)
        {
            Combine(data.Skills, addNewCategories);
        }
    }



    public void Combine(SkillCollection skills, bool addNewCategories = true)
    {
        foreach (var category in skills.Categories)
        {
            var ownCategory = Categories.Find(c => c.Title == category.Title);

            if (ownCategory is null)
            {
                if (addNewCategories)
                {
                    Categories.Add(category.Copy());
                }

                continue;
            }

            ownCategory.Combine(category);
        }
    }



    public static readonly JsonSerializerOptions SkillCollectionJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };



    public static async Task<SkillCollection> LoadSkillsAsync(string filePath)
        => await LoadSkillsAsync(File.OpenRead(filePath));

    public static async Task<SkillCollection> LoadSkillsAsync(Stream stream)
    {
        return await JsonSerializer.DeserializeAsync<SkillCollection>(stream, SkillCollectionJsonOptions) ?? new();
    }



    public override string ToString()
    {
        return $"SkillCollection(Categories: {string.Join('\n', Categories)})";
    }
}
