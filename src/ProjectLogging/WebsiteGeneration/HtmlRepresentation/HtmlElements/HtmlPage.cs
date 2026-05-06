
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class HtmlPage(HeadElement head, IHtmlItem headerRoot, IHtmlItem bodyRoot, IHtmlItem footerRoot) : IHtmlPage
{
    public HeadElement Head { get; set; } = head;
    IHtmlItem IHtmlPage.Head => Head;
    public IHtmlItem Header { get; set; } = headerRoot;
    public IHtmlItem Body { get; set; } = bodyRoot;
    public IHtmlItem Footer { get; set; } = footerRoot;
    public string Title { get => Head.Title; }



    public string GenerateHtml()
    {
        return new StringBuilder()
            .Append("<!DOCTYPE html>")
            .Append(new RawTagElement(HtmlTag.HtmlTags.Html, new StringBuilder()
                    .Append(Head.GenerateHtml())
                    .Append(Header.GenerateHtml())
                    .Append(Body.GenerateHtml())
                    .Append(Footer.GenerateHtml())
                    .ToString())
                .AddAttribute("lang", "en")
                .GenerateHtml())
            .ToString();
    }
}
