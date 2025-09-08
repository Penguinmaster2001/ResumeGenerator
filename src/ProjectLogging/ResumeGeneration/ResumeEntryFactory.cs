
using ProjectLogging.Data;
using ProjectLogging.Skills;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeEntryFactory
{
    public static ResumeEntry CreateEntry(object model) => model switch
    {
        BaseModel baseModel => CreateEntry(baseModel),
        Category category   => CreateEntry(category),
        _ => throw new NotImplementedException($"ResumeEntry factory method not implemented for {model.GetType()}"),
    };



    public static ResumeEntry CreateEntry(BaseModel entryRecord)
    {
        ResumeEntryBuilder entryBuilder = new();

        entryBuilder.SetDescription(entryRecord.ShortDescription)
                    .SetLocation(entryRecord.Location)
                    .SetStartDate(entryRecord.StartDate);

        foreach (string bulletPoint in entryRecord.Points)
        {
            entryBuilder.AddBulletPoint(bulletPoint);
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

        return entryBuilder.GetResumeEntry();
    }



    public static ResumeEntry CreateEntry(string title, IEnumerable<string> items)
        => new ResumeEntryBuilder(title).SetDescription(string.Join(", ", items.Order()))
                                        .GetResumeEntry();



    public static ResumeEntry CreateEntry(Category category) => CreateEntry(category.Name, category.Items);
}
