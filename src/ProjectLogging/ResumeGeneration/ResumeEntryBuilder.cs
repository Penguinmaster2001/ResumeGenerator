
namespace ProjectLogging.ResumeGeneration;



public class ResumeEntryBuilder
{
    private ResumeEntry _resumeEntry;



    public ResumeEntryBuilder(string entryTitle)
    {
        _resumeEntry = new(entryTitle);
    }



    public ResumeEntryBuilder ChangeTitle(string newTitle)
    {
        _resumeEntry.TitleText = newTitle;
        return this;
    }



    public ResumeEntryBuilder SetLocation(string location)
    {
        _resumeEntry.LocationText = location;
        return this;
    }



    public ResumeEntryBuilder ClearLocation()
    {
        _resumeEntry.LocationText = null;
        return this;
    }



    public ResumeEntryBuilder SetStartDate(DateOnly startDate)
    {
        _resumeEntry.StartDate = startDate;
        return this;
    }



    public ResumeEntryBuilder ClearStartDate()
    {
        _resumeEntry.StartDate = null;
        return this;
    }



    public ResumeEntryBuilder SetEndDate(DateOnly endDate)
    {
        _resumeEntry.EndDate = endDate;
        return this;
    }



    public ResumeEntryBuilder ClearEndDate()
    {
        _resumeEntry.EndDate = null;
        return this;
    }



    public ResumeEntryBuilder SetDescription(string description)
    {
        _resumeEntry.DescriptionText = description;
        return this;
    }



    public ResumeEntryBuilder ClearDescription()
    {
        _resumeEntry.DescriptionText = null;
        return this;
    }



    public ResumeEntryBuilder AddBulletPoint(string point)
    {
        _resumeEntry.BulletPointsText.Add(point);
        return this;
    }



    public ResumeEntryBuilder ClearBulletPoints()
    {
        _resumeEntry.BulletPointsText.Clear();
        return this;
    }
}
