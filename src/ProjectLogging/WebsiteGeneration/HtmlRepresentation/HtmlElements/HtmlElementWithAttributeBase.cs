
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public abstract class HtmlElementWithAttributeBase<T>(HtmlTag tag) : IHtmlElement
    where T : HtmlElementWithAttributeBase<T>
{
    protected HtmlTag Tag { get; set; } = tag;



    IHtmlElement IHtmlElement.AddAttribute(string name, string value) => AddAttribute(name, value);

    public T AddAttribute(string name, string value) => AddAttribute(new HtmlTag.Attribute(name, value));



    public T AddAttribute(HtmlTag.Attribute attribute)
    {
        Tag.Attributes.Add(attribute);
        return (T)this;
    }
    IHtmlElement IHtmlElement.AddAttribute(HtmlTag.Attribute attribute) => AddAttribute(attribute);




    public abstract string GenerateHtml();
}
