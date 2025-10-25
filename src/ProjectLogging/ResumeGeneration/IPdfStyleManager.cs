
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public interface IPdfStyleManager
{
    Color PageColor { get; }
    Color SegmentHeaderColor { get; }
    Color HeaderTextColor { get; }
    Color AccentColor { get; }
    Color TextColor { get; }
    List<Color> BulletPointColors { get; }
}
