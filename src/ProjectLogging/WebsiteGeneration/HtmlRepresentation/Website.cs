
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class Website(IFileOrganizer fileOrganizer)
{
    private readonly IFileOrganizer _fileOrganizer = fileOrganizer;

    public List<HtmlPage> Pages { get; set; } = [];



    public async Task CreateFilesAsync()
    {
        Directory.CreateDirectory(_fileOrganizer.RootDirectory);

        var tasks = new List<Task>();

        foreach (var page in Pages)
        {
            tasks.Add(File.WriteAllTextAsync(_fileOrganizer.GetFullPath(page.Head.Title, Constants.Resources.Html),
                page.GenerateHtml()));
        }

        await Task.WhenAll(tasks);
    }
}
