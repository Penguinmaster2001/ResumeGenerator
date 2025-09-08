namespace ProjectLogging.Models.Resume;



public class ResumeModel
{
    public ResumeHeaderModel ResumeHeader { get; set; }
    public ResumeBodyModel ResumeBody { get; set; }


    
    public ResumeModel(ResumeHeaderModel resumeHeader, ResumeBodyModel resumeBody)
    {
        ResumeHeader = resumeHeader;
        ResumeBody = resumeBody;
    }
}
