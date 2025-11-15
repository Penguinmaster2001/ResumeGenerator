
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
        var pageLinker = factory.GetHelper<IPageLinker>();

        var header = new HtmlSection(HtmlTag.Header, [
            IHtmlElement.Div([
                HtmlText.BeginHeader(1, model.ProjectTitle),
                new RawHtml($"<p class=\"tagline\">{model.ShortDescription}</p>")
            ]).AddCssClass("hero-content"),
        ]).AddCssClass("hero");

        var main = new HtmlSection(HtmlTag.Main, [
            new HtmlSection([
                HtmlText.BeginHeader(2, "Overview"),
                HtmlText.BeginParagraph(model.Description ?? string.Empty),
            ]).AddCssClass("overview"),

            new HtmlSection([
                IHtmlElement.Div([
                    HtmlText.BeginHeader(3, "Goals"),
                    IHtmlElement.UnorderedList(model.Goals?.Select(IHtmlElement.Raw) ?? [IHtmlElement.Raw("None")])
                ]),
                IHtmlElement.Div([
                    HtmlText.BeginHeader(3, "Built With"),
                    IHtmlElement.UnorderedList(model.BuiltWith?.Select(IHtmlElement.Raw) ?? [IHtmlElement.Raw("None")])
                ]),
            ]).AddCssClass("details-grid"),

            new HtmlSection([
                HtmlText.BeginHeader(2, "Features"),
                IHtmlElement.Div(model.Features?.Select(f =>
                        IHtmlElement.Div(
                            HtmlText.BeginHeader(4, f[0]),
                            HtmlText.BeginParagraph(f[1])
                        ).AddCssClass($"card {f[1].Replace(' ', '-')}"))
                    ?? []
                ).AddCssClass("feature-cards"),
            ]).AddCssClass("features"),
        ]);

        return new HtmlContainer(header, main);
    }
}
