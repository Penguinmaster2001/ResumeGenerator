
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlPageBuilder(string title = "", string style = "", string favicon = "")
{
    private string _title = title;
    private string _style = style;
    private string _favicon = favicon;
    private IHtmlItem? _body;
    private IHtmlItem? _footer;



    public HtmlPageBuilder Title(string title)
    {
        _title = title;
        return this;
    }



    public HtmlPageBuilder Style(string title)
    {
        _style = title;
        return this;
    }



    public HtmlPageBuilder Favicon(string favicon)
    {
        _favicon = favicon;
        return this;
    }


    public HtmlPageBuilder AddBody(IHtmlItem body)
    {
        _body = body;
        return this;
    }


    public HtmlPageBuilder AddFooter(IHtmlItem footer)
    {
        _footer = footer;
        return this;
    }



    public HtmlPage Build()
    {
        var head = new HeadElement(_title, _style, _favicon);
        var header = new NavHeaderElement();

        return new HtmlPage(head,
                            header,
                            _body ?? new RawHtml(),
                            _footer ?? new RawHtml());
    }
}
