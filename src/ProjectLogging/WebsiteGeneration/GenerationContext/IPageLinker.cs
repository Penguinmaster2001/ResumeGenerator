
namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public interface IPageLinker
{
    List<string> GetPagePaths(List<string> pageLabels);
}
