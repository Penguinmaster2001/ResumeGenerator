
namespace ProjectLogging.ResumeGeneration;



public class ResumeModel
{
    public ResumeHeaderComponent ResumeHeader;
    public List<ResumeSegmentComponent> ResumeBodyComponents;



    public ResumeModel(ResumeHeaderComponent resumeHeader, List<ResumeSegmentComponent> resumeBodyComponents)
    {
        ResumeHeader = resumeHeader;
        ResumeBodyComponents = resumeBodyComponents;
    }
}
