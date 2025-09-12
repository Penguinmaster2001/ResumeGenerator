
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.Views.Html;



public class ResumeBodyHtmlStrategy : ViewStrategy<IHtmlItem, ResumeBodyModel>
{
    public override IHtmlItem BuildView(ResumeBodyModel model, IViewFactory<IHtmlItem> factory)
        => new HtmlSection(HtmlTag.Section, model.ResumeSegments.Select(s => s.CreateView(factory)))
            .AddAttribute("class", "grid-container");
}
