
namespace ProjectLogging;



public interface IFileOrganizer
{
    string RootDirectory { get; set; }



    IEnumerable<PathInfo> QueryName(string resourceName);



    string GetPath(string resourceName, string resourceType);



    string GetRelativePath(string resourceName, string resourceType);



    string GetFullPath(string resourceName, string resourceType);



    PathInfo GetPathInfo(string resourceName, string resourceType);
}
