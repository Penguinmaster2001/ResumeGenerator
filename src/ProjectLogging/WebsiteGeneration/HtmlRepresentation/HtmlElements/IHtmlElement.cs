
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IHtmlElement : IHtmlItem
{
    IHtmlElement AddAttribute(string name, string value);
    IHtmlElement AddAttribute(HtmlTag.Attribute attribute);
}
