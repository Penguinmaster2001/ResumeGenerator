
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration.Styling;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeDocument : IDocument
{
    private readonly IViewFactory<Action<IContainer>> _viewFactory;
    public ResumeModel ResumeModel { get; set; }



    public ResumeDocument(ResumeModel resumeModel, IViewFactory<Action<IContainer>> viewFactory)
    {
        _viewFactory = viewFactory;
        ResumeModel = resumeModel;
    }



    public void Compose(IDocumentContainer container) => container.Page(page =>
        {
            var styleManager = _viewFactory.GetHelper<IPdfStyleManager>();

            page.Size(PageSizes.Letter);
            page.Margin(0.3f, Unit.Inch);
            page.PageColor(styleManager.PageColor);
            page.DefaultTextStyle(textStyle => textStyle.FontSize(11.0f)
                .Weight(FontWeight.Light)
                .FontColor(styleManager.TextColor)
                .FontFamily(styleManager.FontFamily)
                .LineHeight(styleManager.DefaultLineHeight));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeBody);
        });



    void ComposeHeader(IContainer container) => container.Element(_viewFactory.CreateView(ResumeModel.ResumeHeader));



    void ComposeBody(IContainer container) => container.Element(_viewFactory.CreateView(ResumeModel.ResumeBody));
}
