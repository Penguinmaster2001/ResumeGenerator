namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class NavHeaderElement : HtmlElementWithAttributeBase<NavHeaderElement>
{
    public List<HtmlPage> Pages { get; set; } = [];



    public NavHeaderElement() : base(new HtmlTag(HtmlTag.HtmlTags.Header)) { }



    public override string GenerateHtml()
    {
        return $"{Tag.Opener}{Tag.Closer}"; // TODO
    }
}
