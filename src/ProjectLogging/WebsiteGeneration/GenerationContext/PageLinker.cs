

namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public class PageLinker : IPageLinker
{
    private readonly IFileOrganizer _fileOrganizer;



    public PageLinker(IFileOrganizer fileOrganizer)
    {
        _fileOrganizer = fileOrganizer;
    }



    public List<string> GetPagePaths(List<string> pageLabels)
        => [.. pageLabels.Select(p => _fileOrganizer.GetRelativePath(p, Constants.Resources.Html))];
}
