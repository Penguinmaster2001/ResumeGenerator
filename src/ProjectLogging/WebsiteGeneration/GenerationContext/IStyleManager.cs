
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public interface IStyleManager
{
    void AddStyle(IHtmlElement element, string property, string value);



    void ApplyStyle(IHtmlElement element);
}
