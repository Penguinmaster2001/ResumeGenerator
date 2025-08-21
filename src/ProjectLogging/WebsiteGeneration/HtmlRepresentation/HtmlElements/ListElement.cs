
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class ListElement : IHtmlElement
{
    public bool Ordered { get; set; }



    public List<IHtmlElement> ListItems { get; set; } = [];


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
