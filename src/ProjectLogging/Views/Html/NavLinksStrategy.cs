
using ProjectLogging.Models.Website;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class NavLinksStrategy : ViewStrategy<IHtmlItem, NavLinksModel>
{
    public override IHtmlItem BuildView(NavLinksModel model, IViewFactory<IHtmlItem> factory)
    {
        var pageLinker = factory.GetHelper<IPageLinker>();
        var pagePaths = pageLinker.GetPagePaths(model.PagesToLink);

        var section = new HtmlSection(HtmlTag.Nav);

        for (int page = 0; page < pagePaths.Count; page++)
        {
            section.Content.Add(new AnchorElement(pagePaths[page], HtmlText.BeginParagraph(model.PagesToLink[page])));
        }

        return section;
    }
}
