
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.Views.Html;



public class ResumeHtmlStrategy : ViewStrategy<IHtmlItem, ResumeModel>
{
    public override IHtmlItem BuildView(ResumeModel model, IViewFactory<IHtmlItem> factory)
    {
        var section = new HtmlSection(HtmlTag.Section);

        section.Content.Add(factory.BuildView(model.ResumeHeader));
        section.Content.Add(factory.BuildView(model.ResumeBody));

        return section;
    }
}
