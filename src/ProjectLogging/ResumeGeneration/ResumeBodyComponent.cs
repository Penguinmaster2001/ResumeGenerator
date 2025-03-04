
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;



namespace ProjectLogging.ResumeGeneration;



public class ResumeBodyComponent : IComponent
{
    public List<ResumeSegmentComponent> ResumeSegments;



    public ResumeBodyComponent(List<ResumeSegmentComponent> resumeSegments)
    {
        ResumeSegments = resumeSegments;
    }



    public void Compose(IContainer container) => container.Row(row =>
        {
            float margin = 2.0f * 0.25f * 72.0f;
            float spacing = 10.0f;
            float rowItemWidth = 0.5f * (PageSizes.Letter.Width - spacing - margin);

            int segmentPivot = ResumeSegments.Count / 2;
            int end = segmentPivot;

            row.Spacing(spacing);

            for (int rowItemNum = 0; rowItemNum < 2; rowItemNum++)
            {
                row.ConstantItem(rowItemWidth).Column(column =>
                    {
                        int start = rowItemNum * segmentPivot;

                        column.Item().Element(ResumeSegments[start].Compose);
                        ResumeSegments[(start + 1)..end].ForEach(segment =>
                            {
                                column.Item().LineHorizontal(0.5f);
                                column.Item().Element(segment.Compose);
                            });
                    });

                end = ResumeSegments.Count;
            }
        });
}
