
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class HtmlPageBuilder(string title = "", string style = "", string favicon = "")
{
    private string _title = title;
    private string _style = style;
    private string _favicon = favicon;
    private IHtmlItem? _header;
    private IHtmlItem? _body;
    private IHtmlItem? _footer;



    public HtmlPageBuilder Title(string title)
    {
        _title = title;
        return this;
    }



    public HtmlPageBuilder Style(string style)
    {
        _style = style;
        return this;
    }



    public HtmlPageBuilder Favicon(string favicon)
    {
        _favicon = favicon;
        return this;
    }



    public HtmlPageBuilder AddHeader(IHtmlItem header)
    {
        _header = new HtmlSection(HtmlTag.HtmlTags.Header, header);
        return this;
    }



    public HtmlPageBuilder AddBody(IHtmlItem body)
    {
        _body = new HtmlSection(HtmlTag.HtmlTags.Body, body);
        return this;
    }



    public HtmlPageBuilder AddFooter(IHtmlItem footer)
    {
        _footer = new HtmlSection(HtmlTag.HtmlTags.Footer, footer);
        return this;
    }



    public HtmlPage Build()
    {
        var head = new HeadElement(_title, _style, _favicon);

        return new HtmlPage(head,
                            _header ?? new RawHtml(),
                            _body ?? new RawHtml(),
                            _footer ?? new RawHtml());
    }
}
