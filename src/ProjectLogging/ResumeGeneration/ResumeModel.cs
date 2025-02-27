
namespace ProjectLogging.ResumeGeneration;



public class ResumeModel
{
    public ResumeHeaderComponent ResumeHeader;
    public List<ResumeBodyComponent> ResumeBodyComponents;



    public ResumeModel(ResumeHeaderComponent resumeHeader, List<ResumeBodyComponent> resumeBodyComponents)
    {
        ResumeHeader = resumeHeader;
        ResumeBodyComponents = resumeBodyComponents;
    }
}
