
using System.Text.Json;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration.Styling;



public class PdfStyleManager : IPdfStyleManager
{
    public Color PageColor { get; set; } = Colors.White;
    public Color SegmentHeaderColor { get; set; } = Colors.Black;
    public List<Color> SegmentBackgroundColors { get; set; } = [Colors.Transparent];
    public Color ResumeHeaderTextColor { get; set; } = Colors.Black;
    public Color NameTextColor { get; set; } = Colors.Black;
    public Color TextColor { get; set; } = Colors.Black;
    public Color AccentColor { get; set; } = Colors.Grey.Darken2;
    public List<Color> BulletPointColors { get; set; } = [Colors.Grey.Darken4, Colors.Black];



    public string FontFamily { get; set; } = string.Empty;



    public static PdfStyleManager CreateFromConfig(PdfStyleConfig config)
    {
        return new PdfStyleManager()
        {
            PageColor = HexToColor(config.PageColor),
            SegmentHeaderColor = HexToColor(config.SegmentHeaderColor),
            SegmentBackgroundColors = [.. config.SegmentBackgroundColors.Select(HexToColor)],
            ResumeHeaderTextColor = HexToColor(config.ResumeHeaderTextColor),
            NameTextColor = HexToColor(config.NameTextColor),
            AccentColor = HexToColor(config.AccentColor),
            TextColor = HexToColor(config.TextColor),
            BulletPointColors = [.. config.BulletPointColors.Select(HexToColor)],
            FontFamily = "Ubuntu Condensed",
        };
    }



    public static async Task<PdfStyleManager> LoadFromJson(string jsonPath)
    {
        var stream = File.OpenRead(jsonPath);

        var config = await JsonSerializer.DeserializeAsync<PdfStyleConfig>(stream);

        if (config is null) return new();

        return CreateFromConfig(config);
    }



    public PdfStyleConfig ToConfig()
    {
        return new PdfStyleConfig(ColorToHex(PageColor),
            ColorToHex(SegmentHeaderColor),
            [.. SegmentBackgroundColors.Select(ColorToHex)],
            ColorToHex(ResumeHeaderTextColor),
            ColorToHex(NameTextColor),
            ColorToHex(AccentColor),
            ColorToHex(TextColor),
            [.. BulletPointColors.Select(ColorToHex)],
            FontFamily);
    }



    public string DumpToJson()
    {
        return JsonSerializer.Serialize(ToConfig());
    }



    /// <summary>
    /// QuestPdf uses ARGB, while I prefer RGBA. This handles that conversion.
    /// 
    /// Formats:
    ///     R
    ///     RG
    ///     RGB
    ///     RGBA
    ///     RRGGB
    ///     RRGGBB
    ///     RRGGBBA
    ///     RRGGBBAA
    /// </summary>
    /// <param name="hex">
    /// The string to convert.
    /// </param>
    /// <returns>
    /// The color based on the Hex.
    /// </returns>
    private static Color HexToColor(string hex)
    {
        hex = hex.TrimStart('#');

        try
        {
            if (hex.Length == 1) return Color.FromHex(hex + "00"); // R -> R00
            if (hex.Length == 2) return Color.FromHex(hex + '0'); // RG -> RG0
            if (hex.Length == 3) return Color.FromHex(hex); // RGB

            if (hex.Length == 4) return Color.FromHex(hex[3] + hex[0..3]); // RGBA -> ARGB
            if (hex.Length == 5) return Color.FromHex(hex + '0'); // RRGGB -> RRGGB0
            if (hex.Length == 6) return Color.FromHex(hex); // RRGGBB
            if (hex.Length == 7) return Color.FromHex(hex[6] + '0' + hex[0..6]); // RRGGBBA -> A0RRGGBB
            if (hex.Length == 8) return Color.FromHex(hex[6..8] + hex[0..6]); // RRGGBBAA -> AARRGGBB
        }
        catch
        {
            Console.WriteLine($"Bad hex: {hex}");
        }

        return Colors.Black;
    }



    private static string ColorToHex(Color color)
    {
        return $"#{color.Red:X2}{color.Green:X2}{color.Blue:X2}{color.Alpha:X2}";
    }
}
