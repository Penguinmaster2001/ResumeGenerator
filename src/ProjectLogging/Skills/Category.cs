
using ProjectLogging.ResumeGeneration;



namespace ProjectLogging.Skills;



public class Category : IResumeEntryable
{
    public string Name;

    public List<string> Items;



    public Category(string name, IEnumerable<string> items)
    {
        Name = name;

        Items = items.ToList();
    }



    public ResumeEntry ToResumeEntry() => ResumeEntryFactory.CreateEntry(Name, Items);
}
