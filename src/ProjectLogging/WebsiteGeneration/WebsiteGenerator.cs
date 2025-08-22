
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerator
{
    public static Website GenerateWebsite()
    {
        var website = new Website(new WebsiteFileOrganizer());

        website.Pages.Add(new HtmlPageBuilder("test page")
            .AddBody(new HtmlSection(new(HtmlTag.HtmlTags.Body), new RawTagElement(HtmlTag.HtmlTags.Header1, "This is a body"),
                new ListElement(false, new RawTagElement(HtmlTag.HtmlTags.Paragraph, "some text"),
                    new RawTagElement(HtmlTag.HtmlTags.Code, "some more text"),
                    new RawTagElement(HtmlTag.HtmlTags.Header4, "even more text")),
                new ListElement(true, new RawTagElement(HtmlTag.HtmlTags.Header1, "This is an ordered list, h1"),
                    new RawTagElement(HtmlTag.HtmlTags.Header2, "This is header 2"),
                    new RawTagElement(HtmlTag.HtmlTags.Header3, "This is header 3"),
                    new RawTagElement(HtmlTag.HtmlTags.Header4, "This is header 4"),
                    new RawTagElement(HtmlTag.HtmlTags.Header5, "This is header 5"),
                    new RawTagElement(HtmlTag.HtmlTags.Header6, "This is header 6"),
                    new RawTagElement(HtmlTag.HtmlTags.Paragraph, "This is a paragraph"))))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }
}
