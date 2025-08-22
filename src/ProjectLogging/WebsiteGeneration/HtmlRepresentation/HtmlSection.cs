
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlSection(HtmlTag tag, params List<IHtmlItem> content) : IHtmlItem
{
    public HtmlTag Tag { get; set; } = tag;
    public List<IHtmlItem> Content { get; set; } = content;



    public string GenerateHtml()
    {
        var sb = new StringBuilder();
        sb.Append(Tag.Opener);

        foreach (var item in Content)
        {
            sb.Append(item.GenerateHtml());
        }

        sb.Append(Tag.Closer);

        return sb.ToString();
    }
}
