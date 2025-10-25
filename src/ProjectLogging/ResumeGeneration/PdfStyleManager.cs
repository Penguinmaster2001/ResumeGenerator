
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class PdfStyleManager : IPdfStyleManager
{
    public Color PageColor { get; set; }
    public Color SegmentHeaderColor { get; set; }
    public Color HeaderTextColor { get; set; }
    public Color TextColor { get; set; }
    public Color AccentColor { get; set; }
    public List<Color> BulletPointColors { get; set; } = [];
}
