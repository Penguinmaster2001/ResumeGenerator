
namespace ProjectLogging;

public interface IFileOrganizer
{
    string GetPathForResource(string resourceName, string resourceType, string rootDir);
}