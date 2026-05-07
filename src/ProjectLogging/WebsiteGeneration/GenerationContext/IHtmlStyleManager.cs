
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public interface IHtmlStyleManager
{
    Dictionary<string, string> StylePaths { get; }



    void AddStyle(IHtmlElement element, string property, string value);



    void ApplyStyle(IHtmlElement element);
}
