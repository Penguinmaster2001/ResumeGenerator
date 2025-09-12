
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
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}