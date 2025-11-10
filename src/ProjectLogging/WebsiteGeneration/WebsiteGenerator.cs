
using ProjectLogging.Models.Website;
using ProjectLogging.Views.Html;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public static class WebsiteGenerator
{
    public static Website GenerateWebsite(string outDir)
    {
        var fileOrganizer = new WebsiteFileOrganizer
        {
            RootDirectory = outDir,
        };

        var viewFactory = new ViewFactory<IHtmlItem>();
        SetUpFactory(viewFactory, new PageLinker(fileOrganizer));

        var website = new Website(fileOrganizer);

        website.Pages.Add(new HtmlPageBuilder("page1", "styles/styles.css")
            .AddHeader(new NavLinksModel(["page0", "page2"]).CreateView(viewFactory))
            .AddBody(HtmlText.BeginHeader(1, "Page1"))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        website.Pages.Add(new HtmlPageBuilder("page2", "styles/styles.css")
            .AddHeader(new NavLinksModel(["page0"]).CreateView(viewFactory))
            .AddBody(HtmlText.BeginHeader(1, "Page2"))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }



    private static void SetUpFactory(ViewFactory<IHtmlItem> viewFactory, IPageLinker pageLinker)
    {
        viewFactory.AddStrategy<ResumeSegmentHtmlStrategy>();
        viewFactory.AddStrategy<ResumeHeaderHtmlStrategy>();
        viewFactory.AddStrategy<ResumeEntryHtmlStrategy>();
        viewFactory.AddStrategy<ResumeBodyHtmlStrategy>();
        viewFactory.AddStrategy<ResumeHtmlStrategy>();
        viewFactory.AddStrategy<NavLinksStrategy>();

        viewFactory.AddHelper(pageLinker);
        viewFactory.AddHelper<IHtmlStyleManager, HtmlStyleManager>();

        viewFactory.AddPostAction((htmlItem, factory) =>
            {
                if (htmlItem is IHtmlElement htmlElement)
                {
                    factory.GetHelper<IHtmlStyleManager>().ApplyStyle(htmlElement);
                }
            });
    }
}
