
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public interface ITemplateManager
{
    void AddBaseData(object data);
    HtmlTemplate Create(string name, object? data);
}
