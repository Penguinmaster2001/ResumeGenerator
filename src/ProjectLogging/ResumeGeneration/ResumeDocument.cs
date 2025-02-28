
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeDocument : IDocument
{
    public ResumeModel ResumeModel;



    public ResumeDocument(ResumeModel resumeModel)
    {
        ResumeModel = resumeModel;
    }



    public void Compose(IDocumentContainer container) => container.Page(page =>
        {
            page.Size(PageSizes.Letter);
            page.Margin(0.25f, Unit.Inch);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(textStyle => textStyle.FontSize(9.5f).FontFamily("Roboto Condensed"));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeBody);
        });



    void ComposeHeader(IContainer container) => container.Element(ResumeModel.ResumeHeader.Compose);



    void ComposeBody(IContainer container) => container.Dynamic(ResumeModel.ResumeBody);
}
