namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IHtmlElement : IHtmlItem
{
    HtmlTag Tag { get; }
    List<ITagAttribute> Attributes { get; }
    IHtmlElement AddAttribute(string name, string value);
    IHtmlElement AddAttribute(ITagAttribute attribute);



    public static HtmlSection Div(params IEnumerable<IHtmlItem> content) => new(HtmlTag.Div, content);
    public static ListElement OrderedList(params IEnumerable<IHtmlElement> content) => new(true, content);
    public static ListElement UnorderedList(params IEnumerable<IHtmlItem> content) => new(false, content);
    public static AnchorElement Anchor(string href, params IEnumerable<IHtmlElement> content) => new(href, content);
    public static AnchorElement Anchor(string href, string text) => new(href, Raw(text));
    public static RawHtml Raw(string html) => new(html);
}
