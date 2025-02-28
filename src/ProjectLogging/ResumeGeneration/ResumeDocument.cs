
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
            page.Content().Element(ComposeContent);
        });



    void ComposeHeader(IContainer container) => container.Element(ResumeModel.ResumeHeader.Compose);



    void ComposeContent(IContainer container) => container.MultiColumn(multiColumn =>
        {
            multiColumn.Columns(2);

            multiColumn.Spacing(10.0f);

            multiColumn.Content().Column(column =>
                {
                    for (int i = 0; i < ResumeModel.ResumeBodyComponents.Count; i++)
                    {
                        ResumeSegmentComponent bodyComponent = ResumeModel.ResumeBodyComponents[i];

                        if (i > 0)
                        {
                            column.Item().PaddingVertical(2.0f).LineHorizontal(0.5f);
                        }

                        column.Item().Element(bodyComponent.Compose);
                    }
                });
        });
}
