
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class HtmlTag
{
    public enum HtmlTags
    {
        Article,
        Aside,
        Div,
        Main,
        Nav,
        OrderedList,
        UnorderedList,
        ListItem,
        Section,
        Anchor,
        Image,
        Header1,
        Header2,
        Header3,
        Header4,
        Header5,
        Header6,
        Paragraph,
        Code,
        Bold,
        Emphasis,
        Head,
        Header,
        Body,
        Footer,
        Html,
    }



    public static HtmlTag Article => new(HtmlTags.Article);
    public static HtmlTag Aside => new(HtmlTags.Aside);
    public static HtmlTag Div => new(HtmlTags.Div);
    public static HtmlTag Main => new(HtmlTags.Main);
    public static HtmlTag Nav => new(HtmlTags.Nav);
    public static HtmlTag OrderedList => new(HtmlTags.OrderedList);
    public static HtmlTag UnorderedList => new(HtmlTags.UnorderedList);
    public static HtmlTag ListItem => new(HtmlTags.ListItem);
    public static HtmlTag Section => new(HtmlTags.Section);
    public static HtmlTag Anchor => new(HtmlTags.Anchor);
    public static HtmlTag Image => new(HtmlTags.Image);
    public static HtmlTag Header1 => new(HtmlTags.Header1);
    public static HtmlTag Header2 => new(HtmlTags.Header2);
    public static HtmlTag Header3 => new(HtmlTags.Header3);
    public static HtmlTag Header4 => new(HtmlTags.Header4);
    public static HtmlTag Header5 => new(HtmlTags.Header5);
    public static HtmlTag Header6 => new(HtmlTags.Header6);
    public static HtmlTag Paragraph => new(HtmlTags.Paragraph);
    public static HtmlTag Code => new(HtmlTags.Code);
    public static HtmlTag Bold => new(HtmlTags.Bold);
    public static HtmlTag Emphasis => new(HtmlTags.Emphasis);
    public static HtmlTag Head => new(HtmlTags.Head);
    public static HtmlTag Header => new(HtmlTags.Header);
    public static HtmlTag Body => new(HtmlTags.Body);
    public static HtmlTag Footer => new(HtmlTags.Footer);
    public static HtmlTag Html => new(HtmlTags.Html);



    public static HtmlTags TextHeader(int header) => header switch
    {
        6 => HtmlTags.Header6,
        5 => HtmlTags.Header5,
        4 => HtmlTags.Header4,
        3 => HtmlTags.Header3,
        2 => HtmlTags.Header2,
        _ => HtmlTags.Header1,
    };



    public static readonly Dictionary<HtmlTags, string> TagStrings = new() {
        { HtmlTags.Article,         "article" },
        { HtmlTags.Aside,           "aside" },
        { HtmlTags.Div,             "div" },
        { HtmlTags.Main,            "main" },
        { HtmlTags.Nav,             "nav" },
        { HtmlTags.OrderedList,     "ol" },
        { HtmlTags.UnorderedList,   "ul" },
        { HtmlTags.ListItem,        "li" },
        { HtmlTags.Section,         "section" },
        { HtmlTags.Anchor,          "a" },
        { HtmlTags.Image,           "img" },
        { HtmlTags.Header1,         "h1" },
        { HtmlTags.Header2,         "h2" },
        { HtmlTags.Header3,         "h3" },
        { HtmlTags.Header4,         "h4" },
        { HtmlTags.Header5,         "h5" },
        { HtmlTags.Header6,         "h6" },
        { HtmlTags.Paragraph,       "p" },
        { HtmlTags.Code,            "code" },
        { HtmlTags.Bold,            "b" },
        { HtmlTags.Emphasis,        "em" },
        { HtmlTags.Head,            "head" },
        { HtmlTags.Header,          "header" },
        { HtmlTags.Body,            "body" },
        { HtmlTags.Footer,          "footer" },
        { HtmlTags.Html,            "html" },
    };



    public HtmlTags Tag { get; set; }
    public List<ITagAttribute> Attributes { get; set; } = [];
    public string TagString => TagStrings[Tag];
    public string AttributeString => string.Join(' ', Attributes.Select(AttributeToString));
    public string Opener => $"<{TagString} {AttributeString}>";
    public string Closer => $"</{TagString}>";
    public string SelfCloser => $"</{TagString} {AttributeString}>";



    public HtmlTag(HtmlTags tag, params List<ITagAttribute> attributes)
    {
        Tag = tag;
        Attributes = attributes;
    }



    private static string AttributeToString(ITagAttribute attribute)
    {
        var (Name, Value) = attribute.GetNameValue();

        return $"{Name}=\"{Value}\"";
    }
}
