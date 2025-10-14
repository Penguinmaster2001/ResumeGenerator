
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class Website(IFileOrganizer fileOrganizer)
{
    private readonly IFileOrganizer _fileOrganizer = fileOrganizer;

    public List<HtmlPage> Pages { get; set; } = [];



    public void CreateFiles()
    {
        Directory.CreateDirectory(_fileOrganizer.RootDirectory);

        foreach (var page in Pages)
        {
            File.WriteAllText(_fileOrganizer.GetFullPath(page.Head.Title, Constants.Resources.Html),
                page.GenerateHtml());
        }
    }
}
