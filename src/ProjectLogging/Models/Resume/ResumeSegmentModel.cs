
using ProjectLogging.Views.Resume;



namespace ProjectLogging.Models.Resume;



public class ResumeSegmentModel
{
    public string TitleText;
    public List<IResumeView> Entries;



    public ResumeSegmentModel(string title, params IEnumerable<IResumeView> entries)
    {
        TitleText = title;
        Entries = [.. entries];
    }
}