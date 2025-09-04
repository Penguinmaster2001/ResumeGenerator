
using ProjectLogging.Models;



namespace ProjectLogging.Skills;



public class Category : IModel
{
    public string Name;

    public List<string> Items;



    public Category(string name, IEnumerable<string> items)
    {
        Name = name;

        Items = items.ToList();
    }
}
