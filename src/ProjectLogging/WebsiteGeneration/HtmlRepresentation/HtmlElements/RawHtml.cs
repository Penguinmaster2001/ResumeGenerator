
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class RawHtml(string html = "") : IHtmlItem
{
    public string Html { get; set; } = html;



    public virtual string GenerateHtml() => Html;
}
