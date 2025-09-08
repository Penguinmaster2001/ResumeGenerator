
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IHtmlElement : IHtmlItem
{
    List<HtmlTag.Attribute> Attributes { get; }
    IHtmlElement AddAttribute(string name, string value);
    IHtmlElement AddAttribute(HtmlTag.Attribute attribute);
}
