
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public abstract class HtmlElementWithAttributeBase<T>(HtmlTag tag) : IHtmlElement
    where T : HtmlElementWithAttributeBase<T>
{
    public HtmlTag Tag { get; protected set; } = tag;
    public List<ITagAttribute> Attributes { get => Tag.Attributes; }



    IHtmlElement IHtmlElement.AddAttribute(string name, string value) => AddAttribute(name, value);
    public T AddAttribute(string name, string value) => AddAttribute(new TagAttribute(name, value));



    IHtmlElement IHtmlElement.AddAttribute(ITagAttribute attribute) => AddAttribute(attribute);
    public T AddAttribute(ITagAttribute attribute)
    {
        Tag.Attributes.Add(attribute);
        return (T)this;
    }



    public abstract string GenerateHtml();
}
