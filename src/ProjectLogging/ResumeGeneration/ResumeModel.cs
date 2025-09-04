
using ProjectLogging.Views.Resume;

namespace ProjectLogging.ResumeGeneration;



public class ResumeModel
{
    public ResumeHeaderComponent ResumeHeader;
    public ResumeBodyComponent ResumeBody;



    public ResumeModel(ResumeHeaderComponent resumeHeader, ResumeBodyComponent resumeBody)
    {
        ResumeHeader = resumeHeader;
        ResumeBody = resumeBody;
    }
}
