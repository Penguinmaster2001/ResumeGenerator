
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class RawHtml(string html) : IHtmlNodeContent
{
    public string Html { get; set; } = html;



    public string GenerateHtml() => Html;
}
