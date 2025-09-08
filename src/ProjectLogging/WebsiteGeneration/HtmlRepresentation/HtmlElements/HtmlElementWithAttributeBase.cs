
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public abstract class HtmlElementWithAttributeBase<T>(HtmlTag tag) : IHtmlElement
    where T : HtmlElementWithAttributeBase<T>
{
    protected HtmlTag Tag { get; set; } = tag;
    public List<HtmlTag.Attribute> Attributes { get => Tag.Attributes; }



    IHtmlElement IHtmlElement.AddAttribute(string name, string value) => AddAttribute(name, value);
    public T AddAttribute(string name, string value) => AddAttribute(new HtmlTag.Attribute(name, value));



    IHtmlElement IHtmlElement.AddAttribute(HtmlTag.Attribute attribute) => AddAttribute(attribute);
    public T AddAttribute(HtmlTag.Attribute attribute)
    {
        Tag.Attributes.Add(attribute);
        return (T)this;
    }



    public abstract string GenerateHtml();
}
