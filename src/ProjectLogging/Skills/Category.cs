
using System.Text.Json.Serialization;



namespace ProjectLogging.Skills;



public class Category
{
    public string Title { get; set; } = string.Empty;
    public List<string> Items { get; set; } = [];



    [JsonConstructor()]
    public Category(string title, List<string> items)
    {
        Title = title;
        Items = items;
    }



    public Category(string title, IEnumerable<string> items)
    {
        Title = title;

        Items = [.. items];
    }




    public void Add(string item)
    {
        // Cannot use a HashSet
        if (!Items.Contains(item))
        {
            Items.Add(item);
        }
    }




    public void Combine(Category category)
    {
        if (category.Title != Title) return;

        foreach (var item in category.Items)
        {
            Add(item);
        }
    }




    public Category Copy()
    {
        return new Category(Title, Items);
    }




    public override string ToString()
    {
        return $"Category(Title: {Title}, Items: {string.Join(", ", Items)}";
    }
}
