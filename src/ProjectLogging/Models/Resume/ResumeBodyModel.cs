
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Resume;



public class ResumeBodyModel : IModel
{
    public List<ResumeSegmentModel> ResumeSegments;



    public ResumeBodyModel(List<ResumeSegmentModel> resumeSegments)
    {
        ResumeSegments = resumeSegments;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
