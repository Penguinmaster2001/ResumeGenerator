
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class RawHtml(string html = "") : IHtmlItem
{
    public string Html { get; set; } = html;



    public virtual string GenerateHtml() => Html;
}
