
using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public static class ResumeEntryFactory
{
    public static ResumeEntry CreateEntry(IRecord entryRecord)
    {
        ResumeEntryBuilder entryBuilder = new();

        entryBuilder.SetDescription(entryRecord.ShortDescription)
                    .SetLocation(entryRecord.Location)
                    .SetStartDate(entryRecord.StartDate);

        foreach (string bulletpoint in entryRecord.Points)
        {
            entryBuilder.AddBulletPoint(bulletpoint);
        }

        if (entryRecord.EndDate.HasValue)
        {
            entryBuilder.SetEndDate(entryRecord.EndDate.Value);
        }

        if (entryRecord is Job jobEntry)
        {
            entryBuilder.ChangeTitle($"{jobEntry.Position} @ {jobEntry.Company}");
        }
        else if (entryRecord is Volunteer volunteerEntry)
        {
            entryBuilder.ChangeTitle($"{volunteerEntry.Position} @ {volunteerEntry.Organization}");
        }
        else if (entryRecord is Project projectEntry)
        {
            entryBuilder.ChangeTitle(projectEntry.Title);
        }

        return entryBuilder.GetResumeEntry();
    }



    public static ResumeEntry CreateEntry(string title, IEnumerable<string> items)
        => new ResumeEntryBuilder(title).SetDescription(string.Join(", ", items))
                                        .GetResumeEntry();
}
