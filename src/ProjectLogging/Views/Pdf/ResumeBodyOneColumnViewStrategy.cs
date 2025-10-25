
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeBodyOneColumnViewStrategy : ViewStrategy<Action<IContainer>, ResumeBodyModel>
{
    public override Action<IContainer> BuildView(ResumeBodyModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Row(row =>
            {
                float margin = 2.0f * 0.25f * 72.0f;
                float spacing = 10.0f;
                float rowItemWidth = 1.0f * (PageSizes.Letter.Width - spacing - margin);

                row.Spacing(spacing);

                row.ConstantItem(rowItemWidth).Column(column =>
                        {
                            column.Item().Element(model.ResumeSegments[0].CreateView(factory));
                            model.ResumeSegments[1..].ForEach(segment =>
                                {
                                    column.Item().PaddingVertical(4.0f).LineHorizontal(0.5f).LineColor(factory.GetHelper<IPdfStyleManager>().AccentColor);
                                    column.Item().Element(segment.CreateView(factory));
                                });
                        });
            });
}
