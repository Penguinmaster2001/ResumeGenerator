
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration.Styling;
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
            bool bulletPoints = model.PointsText.Count > 0;
            bool oneLine = model.PointsListMode == ResumeEntryModel.ListModes.CommaSeparated
                && !locationAndDate
                && bulletPoints;

            if (oneLine)
            {
                container.PaddingVertical(0.0f).Row(row =>
                {
                    row.AutoItem().Element(Title($"{model.TitleText}:"));
                    row.ConstantItem(3.0f);
                    row.RelativeItem().Element(Description(string.Join(", ", model.PointsText)));
                });

                return;
            }

            container.PaddingVertical(2.0f).Column(column =>
            {

                column.Item().Element(HeaderRow(model));

                if (model.DescriptionText is not null)
                {
                    column.Item().Element(Description(model.DescriptionText));
                }

                if (bulletPoints)
                {
                    column.Item().Element(BulletPoints(model, factory));
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
                row.AutoItem().Text(StringFormatter.FormatLocationText(model.LocationText)).FontSize(11.0f).Bold();
                row.RelativeItem().AlignRight().Text(StringFormatter.FormatDate(model.StartDate, model.EndDate)).FontSize(11.0f).Bold();
            });



    private Action<IContainer> Title(string title)
        => (container) => container.Text(title).FontSize(11.0f).Bold();



    private Action<IContainer> Description(string descriptionText)
        => (container) => container.Text(descriptionText).FontSize(11.0f);



    private Action<IContainer> BulletPoints(ResumeEntryModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Column(column =>
            {
                int colorIndex = 0;
                var bulletColors = factory.GetHelper<IPdfStyleManager>().BulletPointColors;
                foreach (string bulletPoint in model.PointsText)
                {
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(6.0f);
                        row.ConstantItem(3.0f).AlignMiddle().AlignCenter().Svg("Resources/bullet.svg");
                        row.ConstantItem(4.0f);
                        row.RelativeItem().Text(bulletPoint).LineHeight(1.3f).FontColor(bulletColors[colorIndex]);
                    });

                    colorIndex = (colorIndex + 1) % bulletColors.Count;
                }
            });
}
