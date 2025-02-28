
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeBodyComponent : IDynamicComponent
{
    public List<ResumeSegmentComponent> ResumeSegments;



    public ResumeBodyComponent(List<ResumeSegmentComponent> resumeSegments)
    {
        ResumeSegments = resumeSegments;
    }



    public DynamicComponentComposeResult Compose(DynamicContext context)
    {
        return new() { Content = context.CreateElement(container => container.MultiColumn(multiColumn =>
            {
                multiColumn.Columns(2);

                multiColumn.Spacing(10.0f);

                multiColumn.Content().Column(column =>
                    {
                        for (int i = 0; i < ResumeSegments.Count; i++)
                        {
                            ResumeSegmentComponent bodyComponent = ResumeSegments[i];

                            if (i > 0)
                            {
                                column.Item().PaddingVertical(2.0f).LineHorizontal(0.5f);
                            }

                            column.Item().Element(bodyComponent.Compose);
                        }
                    });
            }))
        };
    }
}
