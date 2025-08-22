
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class ListElement(bool ordered = false, params List<IHtmlElement> listItems) : IHtmlElement
{
    public bool Ordered { get; set; } = ordered;
    public List<IHtmlElement> ListItems { get; set; } = listItems;

    private readonly List<HtmlTag.Attribute> _attributes = [];



    public IHtmlElement AddAttribute(string name, string value)
    {
        _attributes.Add(new HtmlTag.Attribute(name, value));
        return this;
    }



    public IHtmlElement AddAttribute(HtmlTag.Attribute attribute)
    {
        _attributes.Add(attribute);
        return this;
    }



    public string GenerateHtml()
    {
        var listTag = new HtmlTag(Ordered ? HtmlTag.HtmlTags.OrderedList : HtmlTag.HtmlTags.UnorderedList);
        var itemTag = new HtmlTag(HtmlTag.HtmlTags.ListItem);

        var sb = new StringBuilder();
        sb.Append(listTag.Opener);

        foreach (var item in ListItems)
        {
            sb.Append(itemTag.Opener)
              .Append(item.GenerateHtml())
              .Append(itemTag.Closer);
        }

        sb.Append(listTag.Closer);

        return sb.ToString();
    }
}
