
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeSegmentViewStrategy : ViewStrategy<Action<IContainer>, ResumeSegmentModel>
{
    public override Action<IContainer> BuildView(ResumeSegmentModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Column(column =>
            {
                column.Item()
                    .PaddingTop(2.0f)
                    .PaddingBottom(2.0f)
                    .Text(model.TitleText.ToUpper())
                    .AlignCenter()
                    .FontSize(12.0f)
                    .Bold()
                    .FontColor(Colors.Green.Darken3);

                foreach (ResumeEntryModel entry in model.Entries)
                {
                    column.Item().Element(entry.CreateView(factory));
                }
            });
}
