
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



    public static string ResourceType(ResourceTypes resourceType) => resourceType.ToString().ToLower();



    public string GetPathForResource(string resourceName, string resourceType, string rootDir)
    {
        var name = Sanitize(resourceName);
        if (_filePaths.TryGetValue(name, out var path)) return path;

        var newPath = Path.Combine(rootDir, Path.ChangeExtension(name, resourceType));

        _filePaths.Add(name, newPath);

        return newPath;
    }



    private static string Sanitize(string path)
    {
        return path.Replace(' ', '_');
    }
}
