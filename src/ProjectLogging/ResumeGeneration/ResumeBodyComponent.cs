
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeBodyComponent : IComponent
{
    public string Title;

    public List<ResumeEntry> Entries = new();

    public ResumeBodyComponent(string title)
    {
        Title = title;
    }



    public void Compose(IContainer container)
    {
        container.MultiColumn(multiColumn =>
            {
                multiColumn.Columns(2);

                multiColumn.Spacing(10.0f);

                multiColumn.Content().Column(column =>
                    {
                        foreach (ResumeEntry entry in Entries)
                        {
                            column.Item().Element(entry.Compose);
                        }
                    });
            });
    }
}
