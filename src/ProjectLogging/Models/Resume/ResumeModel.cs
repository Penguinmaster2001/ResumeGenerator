
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Resume;



public class ResumeModel : IModel
{
    public ResumeHeaderModel ResumeHeader { get; set; }
    public ResumeBodyModel ResumeBody { get; set; }



    public ResumeModel(ResumeHeaderModel resumeHeader, ResumeBodyModel resumeBody)
    {
        ResumeHeader = resumeHeader;
        ResumeBody = resumeBody;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
