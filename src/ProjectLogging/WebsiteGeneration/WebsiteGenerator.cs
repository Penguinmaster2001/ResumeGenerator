
using ProjectLogging.Models;
using ProjectLogging.Skills;
using ProjectLogging.Views.Resume;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerator
{
    public static Website GenerateWebsite(PersonalInfo personalInfo,
        (string SkillsSegmentName, SkillCollection Skills) skillsSegment,
        params IEnumerable<(string Name, IEnumerable<IResumeView> info)> segments)
    {
        var website = new Website(new WebsiteFileOrganizer());

        website.Pages.Add(new HtmlPageBuilder("test page", "styles/styles.css")
            .AddBody(new HtmlSection(new(HtmlTag.HtmlTags.Body), new RawTagElement(HtmlTag.HtmlTags.Header1, "This is a body"),
                new ListElement(false)
                    .AddItem(HtmlText.BeginParagraph("some text"))
                    .AddItem(HtmlText.BeginCode("some more text"))
                    .AddItem(HtmlText.BeginHeader(4, "even more text"))
                    .AddItem(HtmlText.BeginParagraph("This one is ").Em("built")
                        .StartHeader(4, "header 4 ").StartBold().Text("bold ")
                        .StartEm().Text("Both").EndBold().Text(" emphasis")),

                new ListElement(true)
                    .AddItem(HtmlText.BeginHeader(1, "This is an ordered list, h1"))
                    .AddItem(HtmlText.BeginHeader(2, "This is header 2"))
                    .AddItem(HtmlText.BeginHeader(3, "This is header 3"))
                    .AddItem(HtmlText.BeginHeader(4, "This is header 4"))
                    .AddItem(HtmlText.BeginHeader(5, "This is header 5"))
                    .AddItem(HtmlText.BeginHeader(6, "This is header 6"))
                    .AddItem(HtmlText.BeginParagraph("This is a paragraph"))))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }
}
