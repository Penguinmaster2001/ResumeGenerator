
using ProjectLogging.Views.Resume;



namespace ProjectLogging.Models.Resume;



public class ResumeSegmentModel
{
    public string TitleText;
    private List<IResumeView> _entries;



    public ResumeSegmentModel(string title, params IResumeView[] entries)
    {
        TitleText = title;
        _entries = [.. entries];
    }



    public ResumeSegmentModel(string title, IEnumerable<IResumeView> entries)
    {
        TitleText = title;
        _entries = [.. entries];
    }



    public void AddEntries(params IEnumerable<IResumeView> entries) => _entries.AddRange(entries);



    public void ClearEntries() => _entries.Clear();
}