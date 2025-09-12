namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IHtmlElement : IHtmlItem
{
    List<ITagAttribute> Attributes { get; }
    IHtmlElement AddAttribute(string name, string value);
    IHtmlElement AddAttribute(ITagAttribute attribute);
}
