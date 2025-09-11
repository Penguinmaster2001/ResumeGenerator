
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.Html;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerator
{
    public static Website GenerateWebsite(ResumeModel resumeModel)
    {
        var viewFactory = new ViewFactory<IHtmlItem>();
        viewFactory.AddStrategy(new ResumeHtmlStrategy());
        viewFactory.AddStrategy(new ResumeHeaderHtmlStrategy());
        viewFactory.AddStrategy(new ResumeBodyHtmlStrategy());
        viewFactory.AddStrategy(new ResumeSegmentHtmlStrategy());
        viewFactory.AddStrategy(new ResumeEntryHtmlStrategy());

        var website = new Website(new WebsiteFileOrganizer());

        website.Pages.Add(new HtmlPageBuilder("test page", "styles/styles.css")
            .AddBody(viewFactory.BuildView(resumeModel))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }
}
