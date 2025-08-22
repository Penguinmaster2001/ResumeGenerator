
using System.Text;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlPage(HeadElement head, NavHeaderElement headerRoot, IHtmlItem bodyRoot, IHtmlItem footerRoot) : IHtmlItem
{
    public HeadElement Head { get; set; } = head;
    public NavHeaderElement HeaderRoot { get; set; } = headerRoot;
    public IHtmlItem BodyRoot { get; set; } = bodyRoot;
    public IHtmlItem FooterRoot { get; set; } = footerRoot;



    public string GenerateHtml()
    {
        return new StringBuilder()
            .Append("<!DOCTYPE html>")
            .Append(new RawTagElement(HtmlTag.HtmlTags.Html, new StringBuilder()
                    .Append(Head.GenerateHtml())
                    .Append(HeaderRoot.GenerateHtml())
                    .Append(BodyRoot.GenerateHtml())
                    .Append(FooterRoot.GenerateHtml())
                    .ToString())
                .AddAttribute("lang", "en")
                .GenerateHtml())
            .ToString();
    }
}
