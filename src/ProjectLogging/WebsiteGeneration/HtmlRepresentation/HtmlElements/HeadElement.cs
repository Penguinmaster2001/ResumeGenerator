
using System.Text;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class HeadElement(string title, string style, string favicon) : RawTagElement
{
    public string Title { get; set; } = title;
    public string Style { get; set; } = style;
    public string Favicon { get; set; } = favicon;



    public override string GenerateHtml()
    {
        TagType = HtmlTag.HtmlTags.Head;

        Html = new StringBuilder()
            .Append($"<title>{Title}</title>")
            .Append("<meta charset=\"UTF-8\">")
            .Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">")
            .Append($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{Style}\">")
            .Append($"<link rel=\"icon\" type=\"image/png\" href=\"{Favicon}\">")
            .ToString();

        return base.GenerateHtml();
    }
}
