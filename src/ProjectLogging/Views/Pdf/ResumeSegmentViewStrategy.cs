
using ProjectLogging.Models.Resume;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeSegmentViewStrategy : PdfViewStrategy<ResumeSegmentModel>
{
    public override Action<IContainer> BuildView(ResumeSegmentModel model, IPdfViewFactory factory)
        => (container) => container.Column(column =>
            {
                column.Item()
                    .PaddingTop(2.0f)
                    .Text(model.TitleText.ToUpper())
                    .AlignCenter()
                    .FontSize(12.0f)
                    .Bold()
                    .FontColor(Colors.Green.Darken3);

                foreach (ResumeEntryModel entry in model.Entries)
                {
                    column.Item()
                        .PaddingVertical(5.0f)
                        .Element(factory.BuildView(entry));
                }
            });
}
