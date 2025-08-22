
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public abstract class HtmlElementAttributeBase(HtmlTag tag) : IHtmlElement
{
    protected HtmlTag Tag { get; set; } = tag;



    public IHtmlElement AddAttribute(string name, string value)
    {
        Tag.Attributes.Add(new HtmlTag.Attribute(name, value));
        return this;
    }



    public IHtmlElement AddAttribute(HtmlTag.Attribute attribute)
    {
        Tag.Attributes.Add(attribute);
        return this;
    }



    public abstract string GenerateHtml();
}
