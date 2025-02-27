
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeBodyComponent : IComponent
{
    public string Title;

    private List<ResumeEntry> _entries;



    public ResumeBodyComponent(string title, params ResumeEntry[] entries)
    {
        Title = title;
        _entries = entries.ToList();
    }



    public ResumeBodyComponent(string title, IEnumerable<ResumeEntry> entries)
    {
        Title = title;
        _entries = new(entries);
    }



    public void AddEntries(params ResumeEntry[] entries) => _entries.AddRange(entries);

    public void AddEntries(IEnumerable<ResumeEntry> entries) => _entries.AddRange(entries);



    public void ClearEntries() => _entries.Clear();



    public void Compose(IContainer container) => container.Column(column =>
        {
            foreach (ResumeEntry entry in _entries)
            {
                column.Item().Element(entry.Compose);
            }
        });
}
