
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeSegmentComponent : IComponent
{
    public string TitleText;

    private List<ResumeEntry> _entries;



    public ResumeSegmentComponent(string title, params ResumeEntry[] entries)
    {
        TitleText = title;
        _entries = entries.ToList();
    }



    public ResumeSegmentComponent(string title, IEnumerable<ResumeEntry> entries)
    {
        TitleText = title;
        _entries = new(entries);
    }



    public void AddEntries(params ResumeEntry[] entries) => _entries.AddRange(entries);

    public void AddEntries(IEnumerable<ResumeEntry> entries) => _entries.AddRange(entries);



    public void ClearEntries() => _entries.Clear();



    public void Compose(IContainer container) => container.Column(column =>
        {
            column.Item()
                  .PaddingTop(2.0f)
                  .Text(TitleText.ToUpper())
                  .AlignCenter()
                  .FontSize(12.0f)
                  .Bold()
                  .FontColor(Colors.Green.Darken3);

            foreach (ResumeEntry entry in _entries)
            {
                column.Item()
                      .PaddingVertical(5.0f)
                      .Element(entry.Compose);
            }
        });
}
