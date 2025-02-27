
using System.Text;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeEntry : IComponent
{
    public string TitleText;

    public string? LocationText = null;

    public DateOnly? StartDate = null;
    public DateOnly? EndDate = null;

    public string? DescriptionText = null;
    public List<string> BulletPointsText = new();



    public ResumeEntry(string title)
    {
        TitleText = title;
    }



    public void Compose(IContainer container)
    {
        container.Column(column =>
            {
                column.Item().Element(Title);

                if (LocationText is not null || StartDate.HasValue)
                {
                    column.Item().Element(LocationAndDate);
                }

                if (DescriptionText is not null)
                {
                    column.Item().Element(Description);
                }

                if (BulletPointsText.Count > 0)
                {
                    column.Item().Element(BulletPoints);
                }
            });
    }



    private void Title(IContainer container) => container.AlignCenter().Text(TitleText);



    private void LocationAndDate(IContainer container) => container.Row(row =>
        {
            row.RelativeItem().Text(LocationText ?? "");
            row.RelativeItem().Text(FormatDate());
        });



    private string FormatDate()
    {
        if (!StartDate.HasValue) return "";

        StringBuilder sb = new();

        sb.Append(StartDate?.ToString("MMM yyyy"));
        sb.Append(" - ");

        if (EndDate.HasValue)
        {
            if (EndDate > DateOnly.FromDateTime(DateTime.Now))
            {
                sb.Append("exp. ");
            }

            sb.Append(EndDate?.ToString("MMM yyyy"));
        }
        else
        {
            sb.Append("Present");
        }

        return sb.ToString();
    }



    private void Description(IContainer container) => container.Text(DescriptionText);



    private void BulletPoints(IContainer container) => container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(10.0f).Image(Placeholders.Image(128, 128));
                row.ConstantItem(5);
                row.RelativeItem().Text(Placeholders.Label());
            });
        });
}
