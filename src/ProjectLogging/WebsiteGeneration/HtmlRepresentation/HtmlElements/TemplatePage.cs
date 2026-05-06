
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class TemplatePage : IHtmlPage
{
    public IHtmlItem? Head { get; set; }
    public IHtmlItem? Header { get; set; }
    public IHtmlItem? Body { get; set; }
    public IHtmlItem? Footer { get; set; }
    public required string Title { get; set; }



    public string GenerateHtml()
    {
        return new StringBuilder()
            .Append("<!DOCTYPE html>")
            .Append(new RawTagElement(HtmlTag.HtmlTags.Html, new StringBuilder()
                    .Append(Head?.GenerateHtml())
                    .Append(Header?.GenerateHtml())
                    .Append(Body?.GenerateHtml())
                    .Append(Footer?.GenerateHtml())
                    .ToString())
                .AddAttribute("lang", "en")
                .GenerateHtml())
            .ToString();
    }
}
