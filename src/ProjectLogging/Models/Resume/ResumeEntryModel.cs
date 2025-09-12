
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Resume;



public class ResumeEntryModel : IModel
{
    public string TitleText;

    public string? LocationText = null;

    public DateOnly? StartDate = null;
    public DateOnly? EndDate = null;

    public string? DescriptionText = null;
    public List<string> BulletPointsText = [];



    public ResumeEntryModel(string title)
    {
        TitleText = title;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
