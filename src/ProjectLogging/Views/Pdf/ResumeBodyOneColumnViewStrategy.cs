
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration.Styling;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeBodyOneColumnViewStrategy : ViewStrategy<Action<IContainer>, ResumeBodyModel>
{
    public override Action<IContainer> BuildView(ResumeBodyModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Column(column =>
            {
                int segmentNum = 0;
                var segmentBackgroundColors = factory.GetHelper<IPdfStyleManager>().SegmentBackgroundColors;
                column.Item().Background(segmentBackgroundColors[segmentNum]).Element(model.ResumeSegments[0].CreateView(factory));
                model.ResumeSegments[1..].ForEach(segment =>
                    {
                        segmentNum++;
                        column.Item().PaddingVertical(4.0f).LineHorizontal(0.5f).LineColor(factory.GetHelper<IPdfStyleManager>().AccentColor);
                        column.Item().Background(segmentBackgroundColors[segmentNum % segmentBackgroundColors.Count]).Element(segment.CreateView(factory));
                    });
            });
}
