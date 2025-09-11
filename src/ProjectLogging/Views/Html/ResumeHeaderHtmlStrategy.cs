
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class ResumeHeaderHtmlStrategy : ViewStrategy<IHtmlItem, ResumeHeaderModel>
{
    public override IHtmlItem BuildView(ResumeHeaderModel model, IViewFactory<IHtmlItem> factory)
    {
        var section = new HtmlSection(HtmlTag.Section);

        section.Content.Add(Name(model));
        section.Content.Add(ContactRow(model));
        section.Content.Add(URLRow(model));

        return section;
    }



    private IHtmlItem Name(ResumeHeaderModel model) => HtmlText.BeginHeader(1).Bold(model.NameText);



    private IHtmlItem ContactRow(ResumeHeaderModel model)
        => HtmlText.BeginParagraph()
            .Bold(model.PhoneNumberText)
            .Bold(model.EmailText)
            .Bold(model.LocationText);



    private IHtmlItem URLRow(ResumeHeaderModel model)
    {
        var section = new HtmlSection(HtmlTag.Div);

        for (int urlIndex = 0; urlIndex < model.URLs.Count; urlIndex++)
        {
            string url = model.URLs[urlIndex];
            var link = new AnchorElement($"https://{url}", HtmlText.BeginBold(url));

            section.Content.Add(link);
        }

        return section;
    }
}
