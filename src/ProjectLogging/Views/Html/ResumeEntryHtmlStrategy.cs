using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ResumeEntryHtmlStrategy : ViewStrategy<IHtmlItem, ResumeEntryModel>
{
    public override IHtmlItem BuildView(ResumeEntryModel model, IViewFactory<IHtmlItem> factory)
    {
        var section = new HtmlSection(HtmlTag.Section, Title(model)).AddAttribute("class", "grid-item");

        if (model.LocationText is not null || model.StartDate.HasValue)
        {
            section.Content.Add(LocationAndDate(model));
        }

        if (model.DescriptionText is not null)
        {
            section.Content.Add(Description(model));
        }

        if (model.BulletPointsText.Count > 0)
        {
            section.Content.Add(BulletPoints(model));
        }

        return section;
    }



    private IHtmlItem Title(ResumeEntryModel model) => HtmlText.BeginHeader(3).Bold(model.TitleText);



    private IHtmlItem LocationAndDate(ResumeEntryModel model) => HtmlText.BeginHeader(4)
        .Bold($"{model.LocationText ?? string.Empty}\t{StringFormatter.FormatDate(model.StartDate, model.EndDate)}");



    private IHtmlItem Description(ResumeEntryModel model)
        => HtmlText.BeginParagraph(model.DescriptionText ?? string.Empty);



    private IHtmlItem BulletPoints(ResumeEntryModel model)
        => new ListElement(false, model.BulletPointsText.Select(HtmlText.BeginParagraph));
}
