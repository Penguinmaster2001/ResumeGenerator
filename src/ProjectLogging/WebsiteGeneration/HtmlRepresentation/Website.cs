
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



/// <summary>
/// Holds a list of HtmlPage, manages navigation between them
/// </summary>
public class Website(IFileOrganizer fileOrganizer)
{
    private readonly IFileOrganizer _fileOrganizer = fileOrganizer;

    public List<HtmlPage> Pages { get; set; } = [];



    public void CreateFiles()
    {
        foreach (var page in Pages)
        {
            Directory.CreateDirectory(_fileOrganizer.RootDirectory);
            File.WriteAllText(_fileOrganizer.GetFullPath(page.Head.Title, Constants.Resources.Html),
                page.GenerateHtml());
        }
    }
}
