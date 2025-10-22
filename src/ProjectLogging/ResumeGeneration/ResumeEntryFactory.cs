
using ProjectLogging.Data;
using ProjectLogging.Models.Resume;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeEntryFactory
{
    public static ResumeEntryModel CreateEntry(object model) => model switch
    {
        BaseData baseModel => CreateEntry(baseModel),
        Category category => CreateEntry(category),
        _ => throw new NotImplementedException($"ResumeEntry factory method not implemented for {model.GetType()}"),
    };



    public static ResumeEntryModel CreateEntry(BaseData entryRecord)
    {
        ResumeEntryBuilder entryBuilder = new();

        entryBuilder.SetDescription(entryRecord.ShortDescription)
                    .SetLocation(entryRecord.Location)
                    .SetStartDate(entryRecord.StartDate);

        foreach (string bulletPoint in entryRecord.Points)
        {
            entryBuilder.AddPoint(bulletPoint);
        }

        if (entryRecord.EndDate.HasValue)
        {
            entryBuilder.SetEndDate(entryRecord.EndDate.Value);
        }

        switch (entryRecord)
        {
            case Job jobEntry:
                entryBuilder.ChangeTitle($"{jobEntry.Position} @ {jobEntry.Company}");
                break;
            case Volunteer volunteerEntry:
                entryBuilder.ChangeTitle($"{volunteerEntry.Position} @ {volunteerEntry.Organization}");
                break;
            case Project projectEntry:
                entryBuilder.ChangeTitle(projectEntry.Title);
                break;
            case Education educationEntry:
                entryBuilder.ChangeTitle($"{educationEntry.Degree} @ {educationEntry.School}");
                break;
        }

        return entryBuilder.Build();
    }



    public static ResumeEntryModel CreateEntry(string title, IEnumerable<string> items)
        => new ResumeEntryBuilder(title).AddPoints(items).PointsModeCommaSeparated().Build();



    public static ResumeEntryModel CreateEntry(Category category) => CreateEntry(category.Name, category.Items);


    public static ResumeEntryModel DuplicateEntry(ResumeEntryModel entryModel)
    {
        // DESIGN ISSUE: This creates a shallow copy, which means the PointsText list is shared between
        // the original and the copy. If either object modifies the list (Add/Remove/Clear), both will
        // be affected. This violates the expectation of "duplicate" and can lead to subtle bugs.
        // Consider creating a deep copy: "PointsText = new List<string>(entryModel.PointsText)" or
        // using collection expressions: "PointsText = [..entryModel.PointsText]".
        return new ResumeEntryModel(entryModel.TitleText)
        {
            LocationText = entryModel.LocationText,
            StartDate = entryModel.StartDate,
            EndDate = entryModel.EndDate,
            DescriptionText = entryModel.DescriptionText,
            PointsText = entryModel.PointsText,
            PointsListMode = entryModel.PointsListMode,
        };
    }
}
