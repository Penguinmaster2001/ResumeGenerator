
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.Views.Html;



public class TemplateHtmlStrategy<T> : ViewStrategy<IHtmlItem, T>
{
    public string TemplateName { get; set; } = string.Empty;




    public TemplateHtmlStrategy(string templateName)
    {
        TemplateName = templateName;
    }



    public override IHtmlItem BuildView(T model, IViewFactory<IHtmlItem> factory)
    {
        return new HtmlSection(HtmlTag.Main, factory.GetHelper<ITemplateManager>().Create(TemplateName, model!));
    }
}
