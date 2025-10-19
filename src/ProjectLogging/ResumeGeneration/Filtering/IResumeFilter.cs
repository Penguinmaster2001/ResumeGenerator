
using ProjectLogging.Models.Resume;



namespace ProjectLogging.ResumeGeneration.Filtering;



public interface IResumeFilter
{
    List<ResumeSegmentModel> FilterData(List<ResumeSegmentModel> resumeSegments, string jobDescription);
}
