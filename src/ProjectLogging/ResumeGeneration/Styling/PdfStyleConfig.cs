
namespace ProjectLogging.ResumeGeneration.Styling;



public record PdfStyleConfig(string PageColor,
    string SegmentHeaderColor,
    List<string> SegmentBackgroundColors,
    string ResumeHeaderTextColor,
    string NameTextColor,
    string AccentColor,
    string TextColor,
    List<string> BulletPointColors,
    string FontFamily,
    float DefaultLineHeight);
