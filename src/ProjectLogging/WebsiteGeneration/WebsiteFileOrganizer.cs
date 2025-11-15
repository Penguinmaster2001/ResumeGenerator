
using System.Collections.Concurrent;
using System.Text;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteFileOrganizer : IFileOrganizer
{
    private readonly ConcurrentDictionary<string, Dictionary<string, PathInfo>> _filePaths = [];



    public string RootDirectory { get; set; } = Environment.CurrentDirectory;



    public IEnumerable<PathInfo> QueryName(string resourceName)
    {
        if (!_filePaths.TryGetValue(resourceName, out var namePaths)) return [];

        return namePaths.Values;
    }



    public string GetFullPath(string resourceName, string resourceType)
        => Path.Join(RootDirectory, GetRelativePath(resourceName, resourceType));



    public string GetRelativePath(string resourceName, string resourceType)
        => Path.ChangeExtension(GetPath(resourceName, resourceType), resourceType);



    public string GetPath(string resourceName, string resourceType)
        => GetPathInfo(resourceName, resourceType).Path;



    public PathInfo GetPathInfo(string resourceName, string resourceType)
    {
        if (!_filePaths.TryGetValue(resourceName, out var namePaths))
        {
            namePaths = [];
            _filePaths[resourceName] = namePaths;
        }

        if (!namePaths.TryGetValue(resourceType, out var pathInfo))
        {
            pathInfo = CreatePathInfo(resourceName, resourceType);
            namePaths[resourceType] = pathInfo;
        }

        return pathInfo;
    }



    private static string CreatePath(string resourceName, string resourceType)
        => Path.Join(Constants.Resources.Directory(resourceType), resourceName);



    private static PathInfo CreatePathInfo(string resourceName, string resourceType)
        => new(CreatePath(resourceName, resourceType), resourceName, resourceType, false);
}
