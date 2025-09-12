
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.Views.Html;



public class ResumeHtmlStrategy : ViewStrategy<IHtmlItem, ResumeModel>
{
    public override IHtmlItem BuildView(ResumeModel model, IViewFactory<IHtmlItem> factory)
    {
        var section = new HtmlSection(HtmlTag.Section);

        section.Content.Add(model.ResumeHeader.CreateView(factory));
        section.Content.Add(model.ResumeBody.CreateView(factory));

        return section;
    }
}
