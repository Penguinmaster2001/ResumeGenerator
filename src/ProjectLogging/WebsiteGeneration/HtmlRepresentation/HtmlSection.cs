
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlSection(HtmlTag? tag = null, params List<IHtmlItem> content) : IHtmlItem
{
    public HtmlTag? Tag { get; set; } = tag;
    public List<IHtmlItem> Content { get; set; } = content;



    public string GenerateHtml()
    {
        var sb = new StringBuilder();

        foreach (var item in Content)
        {
            sb.Append(item.GenerateHtml());
        }

        if (Tag is not null)
        {
            sb.Insert(0, Tag.Opener);
            sb.Append(Tag.Closer);
        }


        return sb.ToString();
    }
}
