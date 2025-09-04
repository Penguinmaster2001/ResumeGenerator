
using System.Text;
using ProjectLogging.Views.Resume;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class ResumeEntry : IResumeView
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
            row.RelativeItem().Text(LocationText ?? string.Empty).Bold();
            row.RelativeItem().AlignRight().Text(FormatDate()).Bold();
        });



    private string FormatDate()
    {
        if (!StartDate.HasValue) return string.Empty;

        StringBuilder sb = new();

        sb.Append(StartDate.Value.ToString("MMM"))
          .Append(' ');

        if (EndDate.HasValue)
        {
            bool ongoing = EndDate > DateOnly.FromDateTime(DateTime.Now);

            if (StartDate.Value.Year == EndDate.Value.Year && !ongoing)
            {
                if (StartDate.Value.Month == EndDate.Value.Month)
                {
                    return sb.Append(StartDate.Value.ToString("yyyy"))
                             .ToString();
                }

                return sb.Append("- ")
                         .Append(EndDate.Value.ToString("MMM"))
                         .Append(' ')
                         .Append(StartDate.Value.ToString("yyyy"))
                         .ToString();
            }

            sb.Append(StartDate.Value.ToString("yyyy"))
              .Append(" - ");

            if (ongoing)
            {
                sb.Append("exp. ");
            }

            sb.Append(EndDate.Value.ToString("MMM yyyy"));
        }
        else
        {
            sb.Append(StartDate.Value.ToString("yyyy"))
              .Append(" - Present");
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
                    row.ConstantItem(5.0f);
                    row.ConstantItem(3.0f).AlignMiddle().AlignCenter().Image("Resources/bullet.png");
                    row.ConstantItem(5.0f);
                    row.RelativeItem().Text(bulletPoint);
                });
            }
        });
}
