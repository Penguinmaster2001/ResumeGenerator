
using ProjectLogging.Data;
using ProjectLogging.Models.Website;
using ProjectLogging.Views.Html;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public static class WebsiteGenerator
{
    public static async Task<Website> GenerateWebsiteAsync(string outDir, List<ProjectReadme> projectReadmes)
    {
        var projects = projectReadmes.Select(p => new ProjectCard(p)).ToList();

        var fileOrganizer = new WebsiteFileOrganizer
        {
            RootDirectory = outDir,
        };

        var viewFactory = new ViewFactory<IHtmlItem>();
        SetUpFactory(viewFactory, new PageLinker(fileOrganizer));

        var website = new Website(fileOrganizer);

        // website.Pages.Add(new HtmlPageBuilder("page1", "styles/stylesNew.css")
        //     .AddHeader(new NavLinksModel(["page0", "page2"]).CreateView(viewFactory))
        //     .AddBody(IHtmlElement.Div(projects.Select(p => p.CreateView(viewFactory))))
        //     .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
        //     .Build());

        website.Pages.AddRange(await CreateProjectPages(projectReadmes, viewFactory));

        website.Pages.Add(new HtmlPageBuilder("page2", "styles/stylesNew.css")
            .AddHeader(new NavLinksModel(["page0"]).CreateView(viewFactory))
            .AddBody(HtmlText.BeginHeader(1, "Page2"))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build());

        return website;
    }



    private static void SetUpFactory(ViewFactory<IHtmlItem> viewFactory, IPageLinker pageLinker)
    {
        viewFactory.AddStrategy<ProjectInfoHeroHtmlStrategy>();
        viewFactory.AddStrategy<ResumeSegmentHtmlStrategy>();
        viewFactory.AddStrategy<ResumeHeaderHtmlStrategy>();
        viewFactory.AddStrategy<ProjectCardHtmlStrategy>();
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



    private static async Task<List<HtmlPage>> CreateProjectPages(
        List<ProjectReadme> projectReadmes,
        IViewFactory<IHtmlItem> viewFactory)
    {
        var projectCards = new List<ProjectCard>();

        var projectPages = await Task.WhenAll(projectReadmes.Select(async r =>
        {
            var card = new ProjectCard(r);
            projectCards.Add(card);

            var info = await ProjectInfo.CreateFromCardAsync(card);

            return new HtmlPageBuilder(card.ProjectTitle, "styles/project-hero.css")
                .AddHeader(new NavLinksModel(["page0", "page2"]).CreateView(viewFactory))
                .AddBody(info.CreateView(viewFactory))
                .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
                .Build();
        }));

        var projectCardPage = new HtmlPageBuilder("page1", "styles/stylesNew.css")
            .AddHeader(new NavLinksModel(["page0", "page2"]).CreateView(viewFactory))
            .AddBody(IHtmlElement.Div(projectCards.Select(p => p.CreateView(viewFactory))))
            .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
            .Build();

        return [.. projectPages.Append(projectCardPage)];
    }
}
