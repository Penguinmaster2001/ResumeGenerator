
using System.ComponentModel;

namespace ProjectLogging.WebsiteGeneration;



public class WebsiteFileOrganizer : IFileOrganizer
{
    private readonly Dictionary<string, string> _filePaths = [];



    public enum ResourceTypes
    {
        Html,
        Css,
        JS,
    }



    public static string ResourceType(ResourceTypes resourceType) => nameof(resourceType).ToLower();



    public string GetPathForResource(string resourceName, string resourceType, string rootDir)
    {
        if (_filePaths.TryGetValue(resourceName, out var path)) return path;

        var newPath = Path.Combine(rootDir, Path.ChangeExtension(resourceName, resourceType));

        _filePaths.Add(resourceName, newPath);

        return newPath;
    }
}
