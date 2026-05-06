
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public interface ITemplateManager
{
    HtmlTemplate Create(string name, object? data);
}
