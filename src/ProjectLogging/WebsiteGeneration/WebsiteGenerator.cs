
using ProjectLogging.Models.Website;
using ProjectLogging.Views.Website;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerator
{
    public static Website GenerateWebsite()
    {
        var website = new Website(new WebsiteFileOrganizer());

        website.Pages.Add(new HtmlPageBuilder("test page", "styles/styles.css")
            .AddBody(new FrontPageBody()
            {
                Name = "My Name",
                Bio = "This is a long bio. It has a lot of words. And multiple sentences.",
                WebLinks = [
                    ("wiki.archlinux.org", "Arch Wiki"),
                    ("en.wikipedia.org", "Wikipedia"),
                ],
            }.GetHtmlItem())
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }
}
