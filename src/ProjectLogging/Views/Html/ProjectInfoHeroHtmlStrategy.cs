
using ProjectLogging.Models.Website;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ProjectInfoHeroHtmlStrategy : ViewStrategy<IHtmlItem, ProjectInfo>
{
    public override IHtmlItem BuildView(ProjectInfo model, IViewFactory<IHtmlItem> factory)
    {
        var header = new HtmlSection(HtmlTag.Header, [
            new NavLinksModel(["page1"]).CreateView(factory),
            IHtmlElement.Div([
                HtmlText.BeginHeader(1, model.ProjectTitle),
                new RawHtml($"<p class=\"tagline\">{model.ShortDescription}</p>")
            ]).AddCssClass("hero-content"),
        ]).AddCssClass("hero");

        var main = factory.GetHelper<ITemplateManager>().Create("heroTemplate", model);

        return new HtmlContainer(header, main);
    }
}
