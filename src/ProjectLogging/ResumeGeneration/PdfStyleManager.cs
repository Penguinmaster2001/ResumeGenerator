
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



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
}
