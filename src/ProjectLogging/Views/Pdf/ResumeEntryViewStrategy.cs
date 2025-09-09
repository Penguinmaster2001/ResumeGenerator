
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeEntryViewStrategy : PdfViewStrategy<ResumeEntryModel>
{

    public override Action<IContainer> BuildView(ResumeEntryModel model, IPdfViewFactory factory)
        => (container) => container.Column(column =>
            {
                column.Item().Element(Title(model));

                if (model.LocationText is not null || model.StartDate.HasValue)
                {
                    column.Item().Element(LocationAndDate(model));
                }

                if (model.DescriptionText is not null)
                {
                    column.Item().Element(Description(model));
                }

                if (model.BulletPointsText.Count > 0)
                {
                    column.Item().Element(BulletPoints(model));
                }
            });



    private Action<IContainer> Title(ResumeEntryModel model)
        => (container) => container.Text(model.TitleText).Bold();



    private Action<IContainer> LocationAndDate(ResumeEntryModel model)
        => (container) => container.Row(row =>
            {
                row.RelativeItem().Text(model.LocationText ?? string.Empty).Bold();
                row.RelativeItem().AlignRight().Text(StringFormatter.FormatDate(model.StartDate, model.EndDate)).Bold();
            });



    private Action<IContainer> Description(ResumeEntryModel model)
        => (container) => container.Text(model.DescriptionText);



    private Action<IContainer> BulletPoints(ResumeEntryModel model)
        => (container) => container.Column(column =>
            {
                foreach (string bulletPoint in model.BulletPointsText)
                {
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(5.0f);
                        row.ConstantItem(3.0f).AlignMiddle().AlignCenter().Image("Resources/bullet.png");
                        row.ConstantItem(5.0f);
                        row.RelativeItem().Text(bulletPoint);
                    });
                }
            });
}
