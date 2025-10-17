
namespace ProjectLogging.Models.General;



public readonly struct LocationText()
{
    public static readonly LocationText Empty = new();



    public readonly string? City = null;
    public readonly string? Region = null;
    public readonly string? Country = null;

    public readonly bool IsEmpty = true;



    public LocationText(string? city, string? region, string? country) : this()
    {
        City = city;
        Region = region;
        Country = country;

        IsEmpty = string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(Region) && string.IsNullOrWhiteSpace(Country);
    }



    public LocationText(string? locationString) : this()
    {
        if (locationString is null)
        {
            IsEmpty = true;

            Console.WriteLine($"LT ctor {IsEmpty}");

            return;
        }

        var strings = locationString.Split(separator: ',', count: 3);

        if (strings.Length >= 1)
        {
            City = strings[0].Trim();
        }

        if (strings.Length >= 2)
        {
            Region = strings[1].Trim();
        }

        if (strings.Length >= 3)
        {
            Country = strings[2].Trim();
        }

        IsEmpty = string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(Region) && string.IsNullOrWhiteSpace(Country);
    }




    public override string ToString()
    {
        return $"{City ?? "<NULL>"}, {Region ?? "<NULL>"}, {Country ?? "<NULL>"}";
    }
}
