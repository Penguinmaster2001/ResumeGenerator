
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class ListElement(bool ordered = false, params IEnumerable<IHtmlItem> listItems) : IHtmlElement
{
    public bool Ordered { get; set; } = ordered;
    public List<IHtmlItem> ListItems { get; set; } = [.. listItems];
    public List<ITagAttribute> Attributes { get; } = [];



    IHtmlElement IHtmlElement.AddAttribute(string name, string value) => AddAttribute(name, value);
    public ListElement AddAttribute(string name, string value)
    {
        Attributes.Add(new TagAttribute(name, value));
        return this;
    }



    IHtmlElement IHtmlElement.AddAttribute(ITagAttribute attribute) => AddAttribute(attribute);
    public ListElement AddAttribute(ITagAttribute attribute)
    {
        Attributes.Add(attribute);
        return this;
    }



    public ListElement AddItem(IHtmlItem item)
    {
        ListItems.Add(item);
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
