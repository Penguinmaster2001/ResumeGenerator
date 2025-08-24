
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class RawTagElement : HtmlElementWithAttributeBase<RawTagElement>
{
    private HtmlTag.HtmlTags _tagType;
    public HtmlTag.HtmlTags TagType
    {
        get => _tagType;
        set
        {
            Tag = new(value);
            _tagType = value;
        }
    }
    public string Html { get; set; } = string.Empty;



    public RawTagElement() : base(new HtmlTag(HtmlTag.HtmlTags.Paragraph)) { }
    public RawTagElement(HtmlTag.HtmlTags tagType, string html) : base(new HtmlTag(tagType))
    {
        TagType = tagType;
        Html = html;
    }



    public override string GenerateHtml()
    {
        return $"{Tag.Opener}{Html}{Tag.Closer}";
    }
}
