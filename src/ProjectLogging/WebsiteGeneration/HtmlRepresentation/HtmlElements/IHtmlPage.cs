
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IHtmlPage : IHtmlItem
{
    string Title { get; }
    IHtmlItem? Head { get; }
    IHtmlItem? Header { get; }
    IHtmlItem? Body { get; }
    IHtmlItem? Footer { get; }
}
