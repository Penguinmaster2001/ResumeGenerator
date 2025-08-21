
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlNode
{
    public HtmlNode? Parent { get; set; }
    public IHtmlNodeContent Content { get; set; }



    public HtmlNode(IHtmlNodeContent content)
    {
        Content = content;
    }



    public string GenerateHtml()
    {
        return Content.GenerateHtml();
    }
}
