
using System.Text;
using ProjectLogging.Models.General;



namespace ProjectLogging.Views;



public static class StringFormatter
{
    private static DateOnly? _currentDate = null;
    public static DateOnly CurrentDate
    {
        get
        {
            if (_currentDate is not DateOnly date)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
                _currentDate = date;
            }

            return date;
        }
    }


    public static string FormatDate(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is not DateOnly start) return string.Empty;

        StringBuilder sb = new();

        sb.Append(start.ToString("MMM"))
          .Append(' ');

        if (endDate is DateOnly end && end.Month != CurrentDate.Month)
        {
            bool ongoing = end > CurrentDate;

            if (start.Year == end.Year && !ongoing)
            {
                if (start.Month == end.Month)
                {
                    return sb.Append(start.ToString("yyyy"))
                             .ToString();
                }

                return sb.Append("- ")
                         .Append(end.ToString("MMM"))
                         .Append(' ')
                         .Append(start.ToString("yyyy"))
                         .ToString();
            }

            sb.Append(start.ToString("yyyy"))
              .Append(" - ");

            sb.Append(end.ToString("MMM yyyy"));

            if (ongoing)
            {
                sb.Append(" (expected)");
            }
        }
        else
        {
            sb.Append(start.ToString("yyyy"))
              .Append(" - Present");
        }

        return sb.ToString();
    }



    public static string FormatLocationText(LocationText location, bool includeCity = true, bool includeRegion = true, bool includeCountry = false)
    {
        if (location.IsEmpty) return string.Empty;

        StringBuilder sb = new();
        bool empty = true;

        if (includeCity && !string.IsNullOrWhiteSpace(location.City))
        {
            sb.Append(location.City);
            empty = false;
        }

        if (includeRegion && !string.IsNullOrWhiteSpace(location.Region))
        {
            if (!empty)
            {
                sb.Append(", ");
            }

            sb.Append(location.Region);
            empty = false;
        }

        if (includeCountry && !string.IsNullOrWhiteSpace(location.Country))
        {
            if (!empty)
            {
                sb.Append(", ");
            }

            sb.Append(location.Country);
        }

        return sb.ToString();
    }
}
