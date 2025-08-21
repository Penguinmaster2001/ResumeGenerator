
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class TextElement(HtmlTag.HtmlTags textType, string text) : IHtmlElement
{
    public HtmlTag.HtmlTags TextType { get; set; } = textType;
    public string Text { get; set; } = text;



    public string GenerateHtml()
    {
        var tag = new HtmlTag(TextType);
        return $"{tag.Opener}{Text}{tag.Closer}";
    }
}
