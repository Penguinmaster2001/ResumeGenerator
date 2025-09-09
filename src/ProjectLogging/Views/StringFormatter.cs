
using System.Text;



namespace ProjectLogging.Views;



public static class StringFormatter
{
    public static string FormatDate(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is null) return string.Empty;

        StringBuilder sb = new();

        sb.Append(startDate.Value.ToString("MMM"))
          .Append(' ');

        if (endDate.HasValue)
        {
            bool ongoing = endDate > DateOnly.FromDateTime(DateTime.Now);

            if (startDate.Value.Year == endDate.Value.Year && !ongoing)
            {
                if (startDate.Value.Month == endDate.Value.Month)
                {
                    return sb.Append(startDate.Value.ToString("yyyy"))
                             .ToString();
                }

                return sb.Append("- ")
                         .Append(endDate.Value.ToString("MMM"))
                         .Append(' ')
                         .Append(startDate.Value.ToString("yyyy"))
                         .ToString();
            }

            sb.Append(startDate.Value.ToString("yyyy"))
              .Append(" - ");

            if (ongoing)
            {
                sb.Append("exp. ");
            }

            sb.Append(endDate.Value.ToString("MMM yyyy"));
        }
        else
        {
            sb.Append(startDate.Value.ToString("yyyy"))
              .Append(" - Present");
        }

        return sb.ToString();
    }
}
