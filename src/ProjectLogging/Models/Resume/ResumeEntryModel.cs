
using ProjectLogging.Models.General;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Resume;



public class ResumeEntryModel : IModel
{
    public enum ListModes
    {
        Bullets,
        CommaSeparated,
    }



    public string TitleText;

    public LocationText LocationText = LocationText.Empty;

    public DateOnly? StartDate = null;
    public DateOnly? EndDate = null;

    public string? DescriptionText = null;
    public List<string> PointsText = [];
    public ListModes PointsListMode = ListModes.Bullets;



    public ResumeEntryModel(string title)
    {
        TitleText = title;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
