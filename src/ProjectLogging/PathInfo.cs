
namespace ProjectLogging;



public class PathInfo
{
    public string Path { get; set; }
    public string ResourceName { get; set; }
    public string ResourceType { get; set; }
    public bool Exists { get; set; }



    public PathInfo(string path, string resourceName, string resourceType, bool exists)
    {
        Path = path;
        ResourceName = resourceName;
        ResourceType = resourceType;
        Exists = exists;
    }
}
