
namespace ProjectLogging.Models.Resume;



public class ResumeBodyModel
{
    public List<ResumeSegmentModel> ResumeSegments;



    public ResumeBodyModel(List<ResumeSegmentModel> resumeSegments)
    {
        ResumeSegments = resumeSegments;
    }
}
