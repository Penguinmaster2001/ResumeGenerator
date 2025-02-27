
using System.Text.Json;



namespace ProjectLogging.Projects;



public static class ProjectLoader
{
    public static List<Project> LoadProjects(string filepath)
    {
        string jsonText = File.ReadAllText(filepath);

        return JsonSerializer.Deserialize<List<Project>>(jsonText) ?? new();
    }



    public static void WriteProjects(List<Project> projects, string filepath)
    {
        string jsonText = JsonSerializer.Serialize(projects);

        File.WriteAllText(filepath, jsonText);
    }
}
