
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ResumeEntryHtmlStrategy : ViewStrategy<IHtmlItem, ResumeEntryModel>
{
    public override IHtmlItem BuildView(ResumeEntryModel model, IViewFactory<IHtmlItem> factory)
    {
        var styleManager = factory.GetHelper<IStyleManager>();

        var section = new HtmlSection(HtmlTag.Section, Title(model));

        if (!model.LocationText.IsEmpty || model.StartDate.HasValue)
        {
            section.Content.Add(LocationAndDate(model));
        }

        if (model.DescriptionText is not null)
        {
            section.Content.Add(Description(model));
        }

        if (model.pointsText.Count > 0)
        {
            var bulletPoints = BulletPoints(model);
            styleManager.ApplyStyle(bulletPoints);

            section.Content.Add(bulletPoints);
        }

        return section;
    }



    private IHtmlItem Title(ResumeEntryModel model) => HtmlText.BeginHeader(3).Bold(model.TitleText);



    private IHtmlItem LocationAndDate(ResumeEntryModel model) => HtmlText.BeginHeader(4)
        .Bold($"{StringFormatter.FormatLocationText(model.LocationText)}\t{StringFormatter.FormatDate(model.StartDate, model.EndDate)}");



    private IHtmlItem Description(ResumeEntryModel model)
        => HtmlText.BeginParagraph(model.DescriptionText ?? string.Empty);



    private IHtmlElement BulletPoints(ResumeEntryModel model)
        => new ListElement(false, model.pointsText.Select(HtmlText.BeginParagraph));
}
