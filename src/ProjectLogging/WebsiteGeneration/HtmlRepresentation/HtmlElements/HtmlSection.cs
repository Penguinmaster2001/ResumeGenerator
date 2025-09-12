
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public abstract class HtmlSection<T>(HtmlTag tag, IEnumerable<IHtmlItem> content) : HtmlElementWithAttributeBase<T>(tag)
    where T : HtmlSection<T>
{
    public List<IHtmlItem> Content { get; set; } = [.. content];



    public override string GenerateHtml()
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



public class HtmlSection : HtmlSection<HtmlSection>
{
    public HtmlSection(HtmlTag tag, params IEnumerable<IHtmlItem> content) : base(tag, content) { }

    public HtmlSection(HtmlTag.HtmlTags tag, params IEnumerable<IHtmlItem> content) : base(new HtmlTag(tag), content) { }

    public HtmlSection(params IEnumerable<IHtmlItem> content) : base(HtmlTag.Section, content) { }
}
