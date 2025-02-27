
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
            page.Size(PageSizes.A4);
            page.Margin(0.25f, Unit.Inch);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(textStyle => textStyle.FontSize(9.5f));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
        });



    void ComposeHeader(IContainer container) => container.Element(ResumeModel.ResumeHeader.Compose);



    void ComposeContent(IContainer container) => container.MultiColumn(multiColumn =>
        {
            multiColumn.Columns(2);

            multiColumn.Spacing(10.0f);

            multiColumn.Content().Column(column =>
                {
                    foreach (ResumeBodyComponent bodyComponent in ResumeModel.ResumeBodyComponents)
                    {
                        column.Item().Element(bodyComponent.Compose);
                    }
                });
        });
}
