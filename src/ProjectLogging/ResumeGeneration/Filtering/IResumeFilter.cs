
using ProjectLogging.Data;



namespace ProjectLogging.ResumeGeneration.Filtering;



public interface IResumeFilter
{
    public List<(float score, Project project)> FilterData(IDataCollection data, string jobDescription);
}
