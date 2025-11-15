
using ProjectLogging.Models.Website;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ProjectCardHtmlStrategy : ViewStrategy<IHtmlItem, ProjectCard>
{
    public override IHtmlItem BuildView(ProjectCard model, IViewFactory<IHtmlItem> factory)
    {
        var section = IHtmlElement.Div().AddAttribute("class", "project-card");

        section.Content.Add(HtmlText.BeginHeader(3, model.ProjectTitle));
        section.Content.Add(HtmlText.BeginParagraph(model.ShortDescription));
        section.Content.Add(IHtmlElement.UnorderedList().AddAttribute("class", "tags"));
        section.Content.Add(IHtmlElement.Anchor($"{model.ProjectTitle}.html", "View Project").AddAttribute("class", "btn"));

        return section;
    }
}
