
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration.Styling;



public interface IPdfStyleManager
{
    /// <summary>
    /// Color of the page.
    /// </summary>
    Color PageColor { get; }

    /// <summary>
    /// Color of the title of each segment.
    /// </summary>
    Color SegmentHeaderColor { get; }

    /// <summary>
    /// List of colors to cycle through for the background of each segment.
    /// </summary>
    List<Color> SegmentBackgroundColors { get; }

    /// <summary>
    /// Color of the links and contact info at the top of the resume.
    /// </summary>
    Color ResumeHeaderTextColor { get; }

    /// <summary>
    /// Color of the name at the top of the resume.
    /// </summary>
    Color NameTextColor { get; }

    /// <summary>
    /// Color of the lines between segments.
    /// </summary>
    Color AccentColor { get; }

    /// <summary>
    /// Color of all other text.
    /// </summary>
    Color TextColor { get; }

    /// <summary>
    /// List of colors to cycle through for bullet points.
    /// </summary>
    List<Color> BulletPointColors { get; }

    /// <summary>
    /// Font family to use for the resume.
    /// </summary>
    string FontFamily { get; }
}
