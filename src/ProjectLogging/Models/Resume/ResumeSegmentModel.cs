
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Resume;



public class ResumeSegmentModel : IModel
{
    public string TitleText;
    public List<ResumeEntryModel> Entries;



    public ResumeSegmentModel(string title, params IEnumerable<ResumeEntryModel> entries)
    {
        TitleText = title;
        Entries = [.. entries];

        if (TitleText == "Languages")
        {
            Console.WriteLine(string.Join(", ", Entries.Select(e => e.TitleText)));
        }
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}