
using System.ComponentModel;

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
        Link,
        Image,
        Header1,
        Header2,
        Header3,
        Header4,
        Header5,
        Header6,
        Paragraph,
        Code,
        Head,
        Header,
        Body,
        Footer,
        Html,
    }



    public record Attribute(string Name, string Value)
    {
        public override string ToString() => $"{Name}=\"{Value}\"";
    }



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
        { HtmlTags.Link,            "a" },
        { HtmlTags.Image,           "img" },
        { HtmlTags.Header1,         "h1" },
        { HtmlTags.Header2,         "h2" },
        { HtmlTags.Header3,         "h3" },
        { HtmlTags.Header4,         "h4" },
        { HtmlTags.Header5,         "h5" },
        { HtmlTags.Header6,         "h6" },
        { HtmlTags.Paragraph,       "p" },
        { HtmlTags.Code,            "c" },
        { HtmlTags.Head,            "head" },
        { HtmlTags.Header,          "header" },
        { HtmlTags.Body,            "body" },
        { HtmlTags.Footer,          "footer" },
        { HtmlTags.Html,            "html" },
    };



    public HtmlTags Tag { set => TagString = TagStrings[value]; }
    public List<Attribute> Attributes { get; set; } = [];
    public string TagString { get; set; } = string.Empty;
    public string Opener => $"<{TagString} {AttributeString}>";
    public string Closer => $"</{TagString}>";
    public string SelfCloser => $"</{TagString} {AttributeString}>";
    public string AttributeString => string.Join(' ', Attributes);



    public HtmlTag(string tagString, params List<Attribute> attrs)
    {
        TagString = tagString;
        Attributes = attrs;
    }



    public HtmlTag(HtmlTags tag, params List<Attribute> attrs)
    {
        Tag = tag;
        Attributes = attrs;
    }
}
