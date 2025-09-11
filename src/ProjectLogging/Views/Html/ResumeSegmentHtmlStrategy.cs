
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.Views.Html;



public class ResumeSegmentHtmlStrategy : ViewStrategy<IHtmlItem, ResumeSegmentModel>
{
    public override IHtmlItem BuildView(ResumeSegmentModel model, IViewFactory<IHtmlItem> factory)
    {
        var section = new HtmlSection(HtmlTag.Section, HtmlText.BeginHeader(2).Bold(model.TitleText.ToUpper()));

        foreach (ResumeEntryModel entry in model.Entries)
        {
            section.Content.Add(factory.BuildView(entry));
        }

        return section;
    }
}
