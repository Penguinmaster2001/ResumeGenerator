
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



    // DESIGN ISSUE: These are public fields, not properties, which violates encapsulation principles.
    // Fields expose internal implementation details and cannot have validation, change notifications,
    // or be overridden in derived classes. Consider converting to properties with appropriate access
    // modifiers (e.g., "public string TitleText { get; set; }"). This is especially problematic for
    // collection types like PointsText which can be modified externally without class control.
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
