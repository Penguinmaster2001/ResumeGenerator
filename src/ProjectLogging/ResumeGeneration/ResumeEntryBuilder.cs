
using ProjectLogging.Models.Resume;



namespace ProjectLogging.ResumeGeneration;



public class ResumeEntryBuilder
{
    private ResumeEntryModel _resumeEntry;



    public ResumeEntryBuilder(string entryTitle = "")
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
        _resumeEntry.LocationText = new(location);
        return this;
    }



    public ResumeEntryBuilder ClearLocation()
    {
        _resumeEntry.LocationText = new();
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



    public ResumeEntryBuilder SetDescription(string? description)
    {
        _resumeEntry.DescriptionText = description;
        return this;
    }



    public ResumeEntryBuilder ClearDescription()
    {
        _resumeEntry.DescriptionText = null;
        return this;
    }



    public ResumeEntryBuilder AddPoint(string point)
    {
        _resumeEntry.PointsText.Add(point);
        return this;
    }



    public ResumeEntryBuilder AddPoints(IEnumerable<string> points)
    {
        foreach (var point in points)
        {
            AddPoint(point);
        }
        return this;
    }



    public ResumeEntryBuilder ClearBulletPoints()
    {
        _resumeEntry.PointsText.Clear();
        return this;
    }



    public ResumeEntryBuilder PointsModeBullets()
    {
        _resumeEntry.PointsListMode = ResumeEntryModel.ListModes.Bullets;
        return this;
    }



    public ResumeEntryBuilder PointsModeCommaSeparated()
    {
        _resumeEntry.PointsListMode = ResumeEntryModel.ListModes.CommaSeparated;
        return this;
    }



    public ResumeEntryModel Build() => _resumeEntry;
}
