
using ProjectLogging.Models.Resume;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeDocument : IDocument
{
    private readonly IPdfViewFactory _viewFactory;
    public ResumeModel ResumeModel { get; set; }



    public ResumeDocument(ResumeModel resumeModel, IPdfViewFactory viewFactory)
    {
        _viewFactory = viewFactory;
        ResumeModel = resumeModel;
    }



    public void Compose(IDocumentContainer container) => container.Page(page =>
        {
            page.Size(PageSizes.Letter);
            page.Margin(0.25f, Unit.Inch);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(textStyle => textStyle.FontSize(9.5f).FontFamily("Roboto Condensed").LineHeight(1.1f));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeBody);
        });



    void ComposeHeader(IContainer container) => container.Element(_viewFactory.BuildView(ResumeModel.ResumeHeader));



    void ComposeBody(IContainer container) => container.Element(_viewFactory.BuildView(ResumeModel.ResumeBody));
}
