
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeEntryViewStrategy : ViewStrategy<Action<IContainer>, ResumeEntryModel>
{

    public override Action<IContainer> BuildView(ResumeEntryModel model, IViewFactory<Action<IContainer>> factory)
        => (container) =>
        {
            bool locationAndDate = !model.LocationText.IsEmpty || model.StartDate is not null;
            bool bulletPoints = model.pointsText.Count > 0;
            bool oneLine = model.PointsListMode == ResumeEntryModel.ListModes.CommaSeparated
                && !locationAndDate
                && bulletPoints;

            if (oneLine)
            {
                container.Row(row =>
                {
                    row.AutoItem().Element(Title($"{model.TitleText}:"));
                    row.ConstantItem(3.0f);
                    row.RelativeItem().Element(Description(string.Join(", ", model.pointsText)));
                });

                return;
            }

            container.Column(column =>
            {

                column.Item().Element(HeaderRow(model));

                if (model.DescriptionText is not null)
                {
                    column.Item().Element(Description(model.DescriptionText));
                }

                if (bulletPoints)
                {
                    column.Item().Element(BulletPoints(model));
                }
            });
        };



    private Action<IContainer> HeaderRow(ResumeEntryModel model)
        => (container) => container.Row(row =>
            {
                row.AutoItem().Element(Title(model.TitleText));
                row.ConstantItem(5.0f);
                row.ConstantItem(5.0f).AlignMiddle().AlignCenter().Svg("Resources/star.svg");
                row.ConstantItem(5.0f);
                row.AutoItem().Text(StringFormatter.FormatLocationText(model.LocationText)).Bold();
                row.RelativeItem().AlignRight().Text(StringFormatter.FormatDate(model.StartDate, model.EndDate)).Bold();
            });



    private Action<IContainer> Title(string title)
        => (container) => container.Text(title).Bold();



    private Action<IContainer> Description(string descriptionText)
        => (container) => container.Text(descriptionText).FontSize(11.0f);



    private Action<IContainer> BulletPoints(ResumeEntryModel model)
        => (container) => container.Column(column =>
            {
                foreach (string bulletPoint in model.pointsText)
                {
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(5.0f);
                        row.ConstantItem(3.0f).AlignMiddle().AlignCenter().Svg("Resources/bullet.svg");
                        row.ConstantItem(5.0f);
                        row.RelativeItem().Text(bulletPoint);
                    });
                }
            });
}
