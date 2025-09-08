
using ProjectLogging.WebsiteGeneration;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Website;



public class FrontPageBody : IHtmlView<HtmlSection>
{
    public string Name { get; set; } = string.Empty;
    public List<(string Url, string Text)> WebLinks { get; set; } = [];
    public string Bio { get; set; } = string.Empty;



    IHtmlItem IHtmlView.GetHtmlItem() => GetHtmlItem();
    public HtmlSection GetHtmlItem()
    {
        return new HtmlSection(HtmlTag.Body,
            new HtmlSection(HtmlText.BeginHeader(1, Name)).AddAttribute("class", "name"),
            new HtmlSection(new HtmlSection(HtmlTag.Div, WebLinks.Select(l =>
                        new HtmlSection(HtmlTag.Div, new AnchorElement(l.Url, AnchorElement.Targets.Blank, HtmlText.BeginParagraph(l.Text))).AddAttribute("class", "grid-item")
                    )
                ).AddAttribute("class", "grid-container")).AddAttribute("class", "social-links")
            );
    }
}
