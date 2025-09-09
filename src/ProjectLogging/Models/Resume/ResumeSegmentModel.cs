
namespace ProjectLogging.Models.Resume;



public class ResumeSegmentModel
{
    public string TitleText;
    public List<ResumeEntryModel> Entries;



    public ResumeSegmentModel(string title, params IEnumerable<ResumeEntryModel> entries)
    {
        TitleText = title;
        Entries = [.. entries];
    }
}