
namespace ProjectLogging.Models.Resume;



public class ResumeEntryModel
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
}
