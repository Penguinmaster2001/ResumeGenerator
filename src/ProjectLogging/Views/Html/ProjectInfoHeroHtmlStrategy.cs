
using ProjectLogging.Models.Website;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ProjectInfoHeroHtmlStrategy : ViewStrategy<IHtmlItem, ProjectInfo>
{
    public override IHtmlItem BuildView(ProjectInfo model, IViewFactory<IHtmlItem> factory)
    {
        return new HtmlSection(HtmlTag.Main, factory.GetHelper<ITemplateManager>().Create("heroTemplate", model));
    }
}
