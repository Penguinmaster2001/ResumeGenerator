
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



    private void Title(IContainer container) => container.Text(TitleText).Bold();



    private void LocationAndDate(IContainer container) => container.Row(row =>
        {
            row.RelativeItem().Text(LocationText ?? "");
            row.RelativeItem().AlignRight().Text(FormatDate());
        });



    private string FormatDate()
    {
        if (!StartDate.HasValue) return string.Empty;

        StringBuilder sb = new();

        sb.Append(StartDate.Value.ToString("MMM"))
          .Append(" ");

        if (EndDate.HasValue)
        {
            if (StartDate.Value.Year == EndDate.Value.Year)
            {
                if (StartDate.Value.Month == EndDate.Value.Month)
                {
                    return sb.Append(StartDate.Value.ToString("yyyy"))
                             .ToString();
                }

                return sb.Append("- ")
                         .Append(EndDate.Value.ToString("MMM"))
                         .Append(" ")
                         .Append(StartDate.Value.ToString("yyyy"))
                         .ToString();
            }

            sb.Append(StartDate.Value.ToString("yyyy"))
              .Append(" - ");

            if (EndDate > DateOnly.FromDateTime(DateTime.Now))
            {
                sb.Append("exp. ");
            }

            sb.Append(EndDate.Value.ToString("MMM yyyy"));
        }
        else
        {
            sb.Append("- Present");
        }

        return sb.ToString();
    }



    private void Description(IContainer container) => container.Text(DescriptionText);



    private void BulletPoints(IContainer container) => container.Column(column =>
        {
            foreach (string bulletPoint in BulletPointsText)
            {
                column.Item().Row(row =>
                {
                    row.ConstantItem(10.0f).Image(Placeholders.Image(128, 128));
                    row.ConstantItem(5);
                    row.RelativeItem().Text(bulletPoint);
                });
            }
        });
}
