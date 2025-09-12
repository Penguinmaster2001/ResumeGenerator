
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeBodyViewStrategy : ViewStrategy<Action<IContainer>, ResumeBodyModel>
{
    public override Action<IContainer> BuildView(ResumeBodyModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Row(row =>
            {
                float margin = 2.0f * 0.25f * 72.0f;
                float spacing = 10.0f;
                float rowItemWidth = 0.5f * (PageSizes.Letter.Width - spacing - margin);

                int segmentPivot = model.ResumeSegments.Count / 2;
                int end = segmentPivot;

                row.Spacing(spacing);

                for (int rowItemNum = 0; rowItemNum < 2; rowItemNum++)
                {
                    row.ConstantItem(rowItemWidth).Column(column =>
                        {
                            int start = rowItemNum * segmentPivot;

                            column.Item().Element(model.ResumeSegments[start].CreateView(factory));
                            model.ResumeSegments[(start + 1)..end].ForEach(segment =>
                                {
                                    column.Item().LineHorizontal(0.5f);
                                    column.Item().Element(segment.CreateView(factory));
                                });
                        });

                    end = model.ResumeSegments.Count;
                }
            });
}
