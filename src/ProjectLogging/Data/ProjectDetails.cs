
namespace ProjectLogging.Data;



public record ProjectDetails(
    string Title,
    string ShortDescription,
    string Motivation,
    List<string> Goals,
    List<ProjectFeatures> Features,
    List<string> BuiltWith);



public record ProjectFeatures(string Name, ProjectFeatures.Statuses Status)
{
    public enum Statuses
    {
        Planned,
        InProgress,
        Done,
    }
}
