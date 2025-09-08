
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



/// <summary>
/// Holds a list of HtmlPage, manages navigation between them
/// </summary>
public class Website(IFileOrganizer fileOrganizer)
{
    private readonly IFileOrganizer _fileOrganizer = fileOrganizer;

    public List<HtmlPage> Pages { get; set; } = [];



    public void CreateFiles(string outputDir)
    {
        foreach (var page in Pages)
        {
            Directory.CreateDirectory(outputDir);
            File.WriteAllText(_fileOrganizer.GetPathForResource(page.Head.Title,
                WebsiteFileOrganizer.ResourceType(WebsiteFileOrganizer.ResourceTypes.Html),
                    outputDir),
                    page.GenerateHtml());
        }
    }
}
